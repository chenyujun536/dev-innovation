using System.Threading.Tasks;

namespace ShootRate.Interface
{
    public delegate Task SpeechRecognizedDelegate(string text);
    public delegate Task SpeechStateChangeDelegate();

    public interface ISpeechService
    {
        event SpeechRecognizedDelegate SpeechRecognized;
        event SpeechStateChangeDelegate SpeechCancelled;
        event SpeechStateChangeDelegate SessionStopped;

        void StopListenContinuously();
        Task StartListenContinuouslyAsync();
    }

    public interface IGetTimestamp
    {
        string GetFormattedTimestamp();
    }
}