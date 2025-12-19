using SimAddonPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAddon
{
    public static class ReportBuilder
    {

        public static string GenerateReportHeader(string title, ISimAddonPluginCtrl.REPORTFORMAT format)
        {
            StringBuilder report = new StringBuilder();
            switch (format)
            {
                case ISimAddonPluginCtrl.REPORTFORMAT.HTML:
                    report.AppendLine("<html>");
                    report.AppendLine("<head>");
                    report.AppendLine($"<title>{title}</title>");
                    report.AppendLine("</head>");
                    report.AppendLine("<body>");
                    report.AppendLine($"<h1>{title}</h1>");
                    report.AppendLine("<br>");
                    break;
                case ISimAddonPluginCtrl.REPORTFORMAT.MD:
                    report.AppendLine($"# {title}");
                    report.AppendLine();
                    break;
                case ISimAddonPluginCtrl.REPORTFORMAT.JSON:
                    // JSON header could be an opening brace or array, depending on context
                    report.AppendLine("{");
                    report.AppendLine($"  \"title\": \"{title}\",");
                    report.AppendLine("  \"report\": [");
                    break;
                default:
                    throw new NotImplementedException("Report format not implemented");
            }
            return report.ToString();
        }

        public static string GenerateReport(List<ISimAddonPluginCtrl> plugins, ISimAddonPluginCtrl.REPORTFORMAT format)
        {
            StringBuilder report = new StringBuilder();
            foreach (var plugin in plugins)
            {
                if (plugin != null)
                {
                    try
                    {
                        string pluginReport = plugin.getFlightReport(format);
                        report.AppendLine(pluginReport);
                        report.AppendLine(); // Add a newline between plugin reports
                    }
                    catch (Exception ex)
                    {
                        //just skip plugin if error occurs
                    }
                }
            }
            return report.ToString();
        }

        public static string GetImageTag(string imagePath, ISimAddonPluginCtrl.REPORTFORMAT format)
        {
            switch (format)
            {
                case ISimAddonPluginCtrl.REPORTFORMAT.HTML:
                    return $"<img src=\"{imagePath}\" alt=\"Image\" />";
                case ISimAddonPluginCtrl.REPORTFORMAT.MD:
                    return $"![Image]({imagePath})";
                default:
                    throw new NotImplementedException("Report format not implemented");
            }
        }

        public static string GenerateReportFooter(ISimAddonPluginCtrl.REPORTFORMAT format)
        {
            StringBuilder report = new StringBuilder();
            switch (format)
            {
                case ISimAddonPluginCtrl.REPORTFORMAT.HTML:
                    report.AppendLine("</body>");
                    report.AppendLine("</html>");
                    break;
                case ISimAddonPluginCtrl.REPORTFORMAT.MD:
                    // No specific footer for Markdown
                    break;
                case ISimAddonPluginCtrl.REPORTFORMAT.JSON:
                    report.AppendLine("  ]");
                    report.AppendLine("}");
                    break;
                default:
                    throw new NotImplementedException("Report format not implemented");
            }
            return report.ToString();
        }
    }
}
