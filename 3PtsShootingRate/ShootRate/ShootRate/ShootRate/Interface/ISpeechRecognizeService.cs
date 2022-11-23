using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShootRate.Interface
{
    public delegate void RecognizeMatchDelegate(string matchText);

    public delegate void RecognizeSessionDelegate();

    public interface ISpeechRecognizeService : IDisposable
    {
        event RecognizeMatchDelegate RecognizedMatch;
        event RecognizeMatchDelegate RecognizedNoMatch;
        event RecognizeSessionDelegate SessionStarted;
        event RecognizeSessionDelegate SessionStopped;

        Task StartContinuousRecognitionAsync();

        Task StopContinuousRecognitionAsync();

    }

    public interface ITextToSpeech
    {
        Task SpeakAsync(string text);
    }
}
