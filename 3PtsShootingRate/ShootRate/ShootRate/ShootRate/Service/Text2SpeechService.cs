using ShootRate.Interface;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ShootRate.Service
{
    public class Text2SpeechService : ITextToSpeech
    {
        public async Task SpeakAsync(string text)
        {
            await TextToSpeech.SpeakAsync(text);
        }
    }
}