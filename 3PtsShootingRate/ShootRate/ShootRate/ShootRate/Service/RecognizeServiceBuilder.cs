using ShootRate.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace ShootRate.Service
{
    public class RecognizeServiceBuilder
    {
        public static ISpeechRecognizeService BuildForEnvironment()
        {
            switch (DeviceInfo.DeviceType)
            {
                case DeviceType.Unknown:
                    break;
                case DeviceType.Physical:
                    return new SpeechRecognizeService();
                case DeviceType.Virtual:
                    break;
            }

            return new FakeRecognizeService();

        }

        internal static ITextToSpeech BuildTextForSpeech()
        {
            switch (DeviceInfo.DeviceType)
            {
                case DeviceType.Unknown:
                    break;
                case DeviceType.Physical:
                    return new Text2SpeechService();
                case DeviceType.Virtual:
                    break;
            }

            return new FakeText2SpeechService();
        }
    }
}
