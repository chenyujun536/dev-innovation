using ShootRate.Interface;
using System.Threading.Tasks;

namespace ShootRate.Service
{
    public class FakeText2SpeechService : ITextToSpeech
    {
        public Task SpeakAsync(string text)
        {
            return Task.CompletedTask;
        }
    }
}