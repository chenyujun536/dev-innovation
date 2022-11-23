using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ShootRate.Interface;
using ShootRate.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShootRate.Service
{
    internal class SpeechRecognizeService : ISpeechRecognizeService
    {
        private Microsoft.CognitiveServices.Speech.Audio.AudioConfig _audioConfig;
        private SpeechRecognizer _speechRecognizer;

        private object _lockObject = new object();

        public SpeechRecognizeService()
        {
            var speechConfig = SpeechConfig.FromSubscription(Constants.SelectedServiceKey, Constants.SelectedServiceRegion);
            _audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            _speechRecognizer = new SpeechRecognizer(speechConfig, _audioConfig);

            _speechRecognizer.Recognized += (s, e) =>
             {
                 lock (_lockObject)
                 {
                     if (e.Result.Reason == ResultReason.RecognizedSpeech)
                     {
                         RecognizedMatch?.Invoke(e.Result.Text);
                     }
                     else if (e.Result.Reason == ResultReason.NoMatch)
                     {
                         var details = Microsoft.CognitiveServices.Speech.NoMatchDetails.FromResult(e.Result);
                         RecognizedNoMatch?.Invoke(details.ToString());
                     }
                 }
             };

            _speechRecognizer.SessionStarted += (s, e) =>
            {
                lock (_lockObject)
                {
                    SessionStarted();
                }
            };

            _speechRecognizer.SessionStopped += (s, e) =>
            {
                lock (_lockObject)
                {
                    SessionStopped();
                }
            };
        }

        public event RecognizeMatchDelegate RecognizedMatch;
        public event RecognizeMatchDelegate RecognizedNoMatch;
        public event RecognizeSessionDelegate SessionStarted;
        public event RecognizeSessionDelegate SessionStopped;

        public void Dispose()
        {
            _speechRecognizer.Dispose();
            _audioConfig.Dispose();
        }

        public async Task StartContinuousRecognitionAsync()
        {
            await _speechRecognizer?.StartContinuousRecognitionAsync();
        }

        public async Task StopContinuousRecognitionAsync()
        {
            await _speechRecognizer?.StopContinuousRecognitionAsync();
        }
    }
}
