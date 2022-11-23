using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ShootRate.Resources;
using ShootRate.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ShootRate.Service
{
    internal class ForegroundSpeechService
    {
        bool stopLisenningFlag = true;
        public ForegroundSpeechService()
        {
        }

        public bool IsStopped()
        {
            return stopLisenningFlag;
        }

        public void SetStopFlag()
        {
            stopLisenningFlag = true;
        }


        public async Task ListenContinuouslyAsync(GamePlayer gamePlayer)
        {
            stopLisenningFlag = false;

            using (var recognizer = RecognizeServiceBuilder.BuildForEnvironment())
            {
                var stopRecognition = new TaskCompletionSource<int>();

                recognizer.SessionStarted += async () =>
                {
                    await gamePlayer.StartSession();
                    
                };

                recognizer.RecognizedMatch += async (text) =>
                {
                    //try
                    //{
                    //    await recognizer.StopContinuousRecognitionAsync();
                    //}
                    //catch (ApplicationException err)
                    //{
                    //    await App.LogService.LogDebug($"stop recognition service fail {err.HResult}");
                    //    return; // stop fail, means recognizer not in stable state, ignore this result
                    //}

                    if (text == "1." || text.ToLower() == "one.")
                    {
                        await gamePlayer.Hit();
                    }
                    else if (text == "2." || text.ToLower() == "two.")
                    {
                        await gamePlayer.Miss();
                    }
                    else if (text == "3." || text.ToLower() == "three.")
                    {
                        await gamePlayer.Rollback();
                    }
                    else
                    {
                        App.LogService.LogDebug($"[noise] {text}");
                    }

                    //await Task.Delay(Constants.ServiceCloseInterval).ContinueWith(
                    //    async (t) =>
                    //    {
                    //        while (true)
                    //        {
                    //            try
                    //            {
                    //                await recognizer.StartContinuousRecognitionAsync();
                    //                break; //succeed, exit
                    //            }
                    //            catch (ApplicationException err)
                    //            {
                    //                await App.LogService.LogDebug($"start recognition service fail {err.HResult}");
                    //                await Task.Delay(Constants.ServiceCloseInterval); //fail, retry
                    //            }
                    //        }
                    //    });
                };
                

                recognizer.RecognizedNoMatch += (text) =>
                {
                    App.LogService.LogDebug($"[NoMatch] {text}");
                };

                //recognizer.Canceled += async (s, e) =>
                //{
                //    //stopRecognition.TrySetResult(0);
                //    //await TextToSpeech.SpeakAsync("cancelled");
                //};

                recognizer.SessionStopped += async () =>
                {
                    stopRecognition.TrySetResult(0);
                    await gamePlayer.StopSession();
                };

                await recognizer.StartContinuousRecognitionAsync();
                gamePlayer.RecordStatus = "Record Started";
                // Waits for completion. Use Task.WaitAny to keep the task rooted.
                Task.WaitAny(new[]
                {
                    stopRecognition.Task,
                    Task.Run(
                        ()=>
                        {
                            while(!stopLisenningFlag)
                            {
                                Thread.Sleep(100);
                            }
                        }
                    )
                });

                await recognizer.StopContinuousRecognitionAsync();
                gamePlayer.RecordStatus = "Record Stopped";
            }
        }

    }
}