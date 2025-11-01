using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.Json;

namespace SimDataManager
{
    //class to manage connection details for the virtual airline site
    public class SiteConnection
    {
        public string BASEURL { get; set; }

        // Keep a cookie jar so subsequent requests can reuse the session (like a browser)
        private readonly CookieContainer _cookieJar = new CookieContainer();

        // Session token detected after a successful login in the external browser
        public string SessionToken { get; private set; } = string.Empty;
        public string SessionTokenName { get; private set; } = string.Empty;

        // User info returned by the check_session endpoint
        public int? UserId { get; private set; }
        public string UserCallsign { get; private set; } = string.Empty;

        public SiteConnection(string siteURL)
        {
            BASEURL = siteURL;

            SessionToken = string.Empty;
            SessionTokenName = string.Empty;
            UserId = null;
            UserCallsign = string.Empty;

        }

        /// <summary>
        /// Open the system browser to the site's login page and wait for a redirect back to a local callback.
        /// If the site redirects back with a token/session id in the query or POST body, the token will be stored
        /// in SessionToken and also added to the internal cookie jar for future requests.
        /// Returns the captured token (string) when the flow completed, empty string on timeout or error.
        /// Note: This requires that the site accepts a return/redirect/next/callback parameter pointing to a localhost URL
        /// and will redirect the browser there after a successful login.
        /// </summary>
        /// <param name="timeoutSeconds">Number of seconds to wait for the browser callback (default 120s)</param>
        public async Task<string> Login(int timeoutSeconds = 120)
        {
            if (string.IsNullOrWhiteSpace(BASEURL))
            {
                Logger.WriteLine("Login: BASEURL not set.");
                return string.Empty;
            }

            Logger.WriteLine($"Login: opening login page {BASEURL.TrimEnd('/')}/login.php in default browser");

            // Prepare a local callback listener
            int port;
            try
            {
                port = GetFreePort();
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Login: could not find free port: " + ex.Message);
                return string.Empty;
            }

            string state = Guid.NewGuid().ToString("N");
            string callbackPrefix = $"http://127.0.0.1:{port}/simaddon-callback/"; // must end with '/'
            string callbackUrl = callbackPrefix + "?token=" + Uri.EscapeDataString(state);
            string phpUrl = BASEURL.TrimEnd('/') + "/login.php";

            // Build login URL with common redirect parameter names (server may honor one of them)
            string loginUrl = phpUrl + "?" +
                                "token=" + Uri.EscapeDataString(state) + "&" +
                              "redirect=" + Uri.EscapeDataString(callbackUrl);

            using (var listener = new HttpListener())
            {
                try
                {
                    listener.Prefixes.Add(callbackPrefix);
                    listener.Start();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Login: failed to start local listener: " + ex.Message);
                    return string.Empty;
                }

                // Open the default browser to the login URL
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = loginUrl,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Login: could not open browser: " + ex.Message);
                    try { listener.Stop(); } catch { }
                    return string.Empty;
                }

                // Wait for incoming callback or timeout
                Task<HttpListenerContext> contextTask = listener.GetContextAsync();
                Task delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
                Task completed = await Task.WhenAny(contextTask, delayTask).ConfigureAwait(false);

                if (completed == delayTask)
                {
                    Logger.WriteLine("Login: timeout waiting for browser callback");
                    try { listener.Stop(); } catch { }
                    return string.Empty;
                }

                HttpListenerContext context;
                try
                {
                    context = await contextTask.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Login: error receiving callback: " + ex.Message);
                    try { listener.Stop(); } catch { }
                    return string.Empty;
                }

                try
                {
                    // Read query string
                    var req = context.Request;
                    var qs = req.QueryString;

                    string foundKey = null;
                    string foundValue = null;

                    // Common token keys
                    string[] keysToCheck = new[] { "token", "session", "PHPSESSID", "sid", "sessionid", "sessid", "auth", "access_token" };
                    foreach (var k in keysToCheck)
                    {
                        var v = qs[k];
                        if (!string.IsNullOrEmpty(v))
                        {
                            foundKey = k;
                            foundValue = v;
                            break;
                        }
                    }

                    // If not found in GET params, and method is POST, try to read body
                    if (foundValue == null && string.Equals(req.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                    {
                        using var reader = new StreamReader(req.InputStream, req.ContentEncoding);
                        string body = await reader.ReadToEndAsync().ConfigureAwait(false);
                        if (!string.IsNullOrEmpty(body))
                        {
                            // Parse form-encoded body
                            var pairs = body.Split('&', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var pair in pairs)
                            {
                                var idx = pair.IndexOf('=');
                                if (idx <= 0) continue;
                                var k = Uri.UnescapeDataString(pair.Substring(0, idx));
                                var v = Uri.UnescapeDataString(pair.Substring(idx + 1));
                                if (keysToCheck.Contains(k, StringComparer.OrdinalIgnoreCase))
                                {
                                    foundKey = k;
                                    foundValue = v;
                                    break;
                                }
                            }
                        }
                    }

                    // If we found a token, store it. If not, fall back to the state value we originally sent.
                    SessionToken = !string.IsNullOrEmpty(foundValue) ? foundValue : state;
                    SessionTokenName = "simaddon_token"; // server expects this cookie name

                    Logger.WriteLine($"Login: captured session token {SessionTokenName}={SessionToken} (source key: {foundKey ?? "(state fallback)"})");

                    // Add cookie for the site domain into the internal cookie jar so subsequent HttpClient calls include it
                    try
                    {
                        var baseUri = new Uri(BASEURL);
                        var cookieBase = new Uri($"{baseUri.Scheme}://{baseUri.Host}/");
                        var cookie = new Cookie(SessionTokenName, SessionToken) { Path = "/" };
                        _cookieJar.Add(cookieBase, cookie);
                        Logger.WriteLine("Login: session cookie added to internal cookie jar for " + cookieBase);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("Login: could not add cookie to jar: " + ex.Message);
                    }

                    // Send a small response to the browser
                    try
                    {
                        context.Response.ContentType = "text/html; charset=utf-8";
                        string responseHtml = "<html><head><meta charset=\"utf-8\"/><title>SimAddon</title></head><body style=\"font-family:Segoe UI,Arial,sans-serif;margin:40px;text-align:center;\">" +
                                              "<h2>Connexion détectée</h2><p>Vous pouvez fermer cette fenêtre et revenir à SimAddon.</p></body></html>";
                        byte[] buffer = Encoding.UTF8.GetBytes(responseHtml);
                        context.Response.ContentLength64 = buffer.Length;
                        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                        context.Response.OutputStream.Close();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("Login: error sending response to browser: " + ex.Message);
                    }
                }
                finally
                {
                    try { listener.Stop(); } catch { }
                }

                // Return the token we stored (may be the original state if server did not include a different token)
                return SessionToken;
            }
        }

        private static int GetFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public async void Logout()
        {
            string phpUrl = BASEURL.TrimEnd('/') + "/logout.php";
            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieJar,
                UseCookies = true,
                AllowAutoRedirect = true
            };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT) SimAddon/1.0");
                try
                {
                    using var response = await client.GetAsync(phpUrl).ConfigureAwait(false);
                    Logger.WriteLine("Logout response status: " + response.StatusCode);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Erreur lors de la déconnexion : " + ex.Message);
                }
            }

            // Clear stored session token
            SessionToken = string.Empty;
            SessionTokenName = string.Empty;
            try
            {
                // Create a new cookie jar to forget cookies
                // Note: HttpClient instances that used the old _cookieJar will still have their own copy; creating a fresh jar avoids reuse
                // for subsequent new HttpClientHandlers we create using this instance.
                // We keep the field readonly, so we can't replace it; instead clear entries by enumerating domains is not exposed —
                // so for simplicity we leave it and rely on logout request above to invalidate server session.
            }
            catch { }
        }

        /// <summary>
        /// Check session on the server. Optionally provide a token (e.g. returned by Login) - the method will
        /// add the token as a cookie and also append it to the request as a GET parameter so the server can pick it
        /// using $_COOKIE['simaddon_token'] or $_GET['token'].
        /// </summary>
        public async Task<bool> CheckSession(string token = null)
        {
            if (string.IsNullOrWhiteSpace(BASEURL))
            {
                Logger.WriteLine("CheckSession: BASEURL not set.");
                return false;
            }

            // If a token is provided, store it locally and make sure the cookie jar contains it
            if (!string.IsNullOrEmpty(token))
            {
                SessionToken = token;
                SessionTokenName = "simaddon_token";
                try
                {
                    var baseUri = new Uri(BASEURL);
                    var cookieBase = new Uri($"{baseUri.Scheme}://{baseUri.Host}/");
                    var cookie = new Cookie(SessionTokenName, SessionToken) { Path = "/" };
                    _cookieJar.Add(cookieBase, cookie);
                    Logger.WriteLine("CheckSession: added session cookie to jar for " + cookieBase);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("CheckSession: could not add cookie to jar: " + ex.Message);
                }
            }

            string phpUrl = BASEURL.TrimEnd('/') + "/api/api_check_session.php";

            // If we have a token, also append it as a GET parameter so the server can use $_GET['token'] fallback
            if (!string.IsNullOrEmpty(SessionToken))
            {
                phpUrl += (phpUrl.Contains("?") ? "&" : "?") + "token=" + Uri.EscapeDataString(SessionToken);
            }

            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieJar,
                UseCookies = true,
                AllowAutoRedirect = false // we want to detect redirects
            };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT) SimAddon/1.0");
                try
                {
                    using var response = await client.GetAsync(phpUrl).ConfigureAwait(false);
                    string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        // Detect common redirect to login page
                        if ((response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.SeeOther || response.StatusCode == HttpStatusCode.TemporaryRedirect)
                            && response.Headers.Location != null)
                        {
                            if (response.Headers.Location.ToString().IndexOf("login.php", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Logger.WriteLine("CheckSession: server redirected to login page.");
                                UserId = null;
                                UserCallsign = string.Empty;
                                return false;
                            }
                        }

                        Logger.WriteLine("CheckSession: non-success status " + response.StatusCode + ", body length " + (body?.Length ?? 0));
                        // continue to try to parse body if any
                    }

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        try
                        {
                            using JsonDocument doc = JsonDocument.Parse(body);
                            var root = doc.RootElement;
                            if (root.ValueKind == JsonValueKind.Object)
                            {
                                if (root.TryGetProperty("authenticated", out var a))
                                {
                                    bool authenticated = false;
                                    if (a.ValueKind == JsonValueKind.True) authenticated = true;
                                    else if (a.ValueKind == JsonValueKind.False) authenticated = false;
                                    else if (a.ValueKind == JsonValueKind.Number) authenticated = a.GetInt32() != 0;

                                    if (authenticated)
                                    {
                                        // extract user
                                        if (root.TryGetProperty("user", out var user) && user.ValueKind == JsonValueKind.Object)
                                        {
                                            if (user.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.Number)
                                            {
                                                try { UserId = id.GetInt32(); } catch { UserId = null; }
                                            }
                                            else
                                            {
                                                UserId = null;
                                            }

                                            if (user.TryGetProperty("callsign", out var cs) && cs.ValueKind == JsonValueKind.String)
                                            {
                                                UserCallsign = cs.GetString() ?? string.Empty;
                                            }
                                            else
                                            {
                                                UserCallsign = string.Empty;
                                            }
                                        }

                                        Logger.WriteLine($"CheckSession: authenticated user id={UserId} callsign='{UserCallsign}'");
                                        return true;
                                    }
                                    else
                                    {
                                        Logger.WriteLine("CheckSession: authenticated=false");
                                        UserId = null;
                                        UserCallsign = string.Empty;
                                        return false;
                                    }
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            Logger.WriteLine("CheckSession: invalid JSON response: " + ex.Message);
                        }
                    }

                    // fallback: if we got HTTP 200 and no explicit JSON, assume session valid
                    if (response.IsSuccessStatusCode)
                    {
                        Logger.WriteLine("CheckSession: HTTP 200 with no JSON authenticated field; assuming session valid.");
                        return true;
                    }

                    UserId = null;
                    UserCallsign = string.Empty;
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("CheckSession: error checking session: " + ex.Message);
                    UserId = null;
                    UserCallsign = string.Empty;
                    return false;
                }
            }
        }
    }
}
