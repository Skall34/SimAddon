using System.Speech.AudioFormat;
using System.Speech.Synthesis;

namespace TextToSpeech
{
    public class Speaker : IDisposable
    {
        private readonly SpeechSynthesizer _synthesizer;

        private string lastPhrase;
        private int noMuteVolume;

        private const string NOVOICE = "Aucune";
        public Speaker()
        {
            _synthesizer = new SpeechSynthesizer();
            lastPhrase = "Hello !";
            noMuteVolume = _synthesizer.Volume;
            
            //for test & share, uncomment to set output to wav file.
            //_synthesizer.SetOutputToWaveFile(@"F:\temp\annonce.wav",
            //new SpeechAudioFormatInfo(32000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
        }

        /// <summary>
        /// Speaks the provided text asynchronously.
        /// </summary>
        /// <param name="text">The text to be spoken.</param>
        public void Say(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {

                // Stop any ongoing speech before starting new text
                if (_synthesizer.State == SynthesizerState.Speaking)
                {
                    _synthesizer.SpeakAsyncCancelAll();
                }

                _synthesizer.SpeakAsync(text);
                lastPhrase = text;
            }
        }

        public List<string> GetVoices()
        {
            List<string> result = new List<string>();
             foreach(InstalledVoice v in  _synthesizer.GetInstalledVoices())
            {
                result.Add(v.VoiceInfo.Name);
            }
            result.Add(NOVOICE);
            return result;
        }

        public string GetCurrentVoice()
        {
            string result = NOVOICE;
            if (_synthesizer.Volume > 0)
            {
                result = _synthesizer.Voice.Name;
            }
            return result;
        }

        public void SetVoice(string voiceName)
        {
            if ( voiceName!=NOVOICE)
            {
                SetVolume(noMuteVolume);
                _synthesizer.SelectVoice(voiceName);
                //re-say the last phrase
                Say(lastPhrase);
            }
            else
            {
                noMuteVolume = _synthesizer.Volume;
                SetVolume(0);
            }
        }

        /// <summary>
        /// Stops the current speech, if any.
        /// </summary>
        public void Stop()
        {
            if (_synthesizer.State == SynthesizerState.Speaking)
            {
                _synthesizer.SpeakAsyncCancelAll();
            }
        }

        /// <summary>
        /// Sets the volume of the speech synthesizer.
        /// </summary>
        /// <param name="volume">Volume level (0 to 100).</param>
        public void SetVolume(int volume)
        {
            if (volume < 0 || volume > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 100.");
            }
            Stop();
            _synthesizer.Volume = volume;
        }

        /// <summary>
        /// Sets the rate of speech.
        /// </summary>
        /// <param name="rate">Rate level (-10 to 10).</param>
        public void SetRate(int rate)
        {
            if (rate < -10 || rate > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(rate), "Rate must be between -10 and 10.");
            }

            _synthesizer.Rate = rate;
        }

        /// <summary>
        /// Releases resources used by the speaker.
        /// </summary>
        public void Dispose()
        {
            _synthesizer.Dispose();
        }
    }
}
