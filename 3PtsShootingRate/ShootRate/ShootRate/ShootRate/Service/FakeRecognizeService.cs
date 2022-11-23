using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShootRate.Interface
{
    internal class FakeRecognizeService : ISpeechRecognizeService
    {
        public event RecognizeMatchDelegate RecognizedMatch;
        public event RecognizeMatchDelegate RecognizedNoMatch;
        public event RecognizeSessionDelegate SessionStarted;
        public event RecognizeSessionDelegate SessionStopped;

        private Random random = new Random();
        private bool _stoppedFlag = false;

        public void Dispose()
        {            
        }

        public async Task StartContinuousRecognitionAsync()
        {
            SessionStarted();

            await Task.Delay(1000);

            Task.Run(() =>
              {
                  while (!_stoppedFlag)
                  {
                      Thread.Sleep(5000);
                      RecognizedMatch(random.Next(1, 3).ToString() + ".");
                  }
              });
        }

        public async Task StopContinuousRecognitionAsync()
        {
            _stoppedFlag = true;
            await Task.Delay(1000);
            SessionStopped();
        }
    }
}