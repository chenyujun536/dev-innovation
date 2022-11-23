using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ShootRate.Interface;
using ShootRate.Models;
using ShootRate.Resources;
using ShootRate.Service;
using ShootRate.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShootRate.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class GameEntryPage : ContentPage
    {
        readonly GamePlayer gamePlayer = DependencyService.Get<IGamePlayer>() as GamePlayer;
        ForegroundSpeechService speechService = new ForegroundSpeechService();
        public GameEntryPage()
        {
            InitializeComponent();
            //Task.Run(() => InitSpeechRecognizeServicAsync());
            // Set the BindingContext of the page to a new Note.
            Game game = new Game() { Date = DateTime.Now };
            gamePlayer.InitWith(game);
            BindingContext = gamePlayer;
            HitsListView.ItemsSource = gamePlayer.Hits;
        }

        public string ItemId
        {
            set
            {
                LoadGame(value);
            }
        }

        async void LoadGame(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                // Retrieve the note and set it as the BindingContext of the page.
                Game game = await App.Database.GetNoteAsync(id);
                gamePlayer.InitWith(game);
                BindingContext = gamePlayer;
                HitsListView.ItemsSource = gamePlayer.Hits;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load note.");
            }
        }

        private async Task InitSpeechRecognizeServicAsync()
        {
            await speechService.ListenContinuouslyAsync(gamePlayer);
            //await gamePlayer.ListenContinuouslyAsync();
        }

        async void OnRecordButtonClicked(object sender, EventArgs e)
        {
            if(speechService.IsStopped())
            {
                var answer = await DisplayActionSheet("Select service key", "Cancel", null, Constants.ServiceKeyFreeTrial, Constants.ServiceKeyBGC541, Constants.ServiceKeyBGC541FreeTier);

                if (answer == "Cancel")
                    return;
                else
                {
                    App.LogService.LogInfo($"start recording with service key {answer}");
                    Constants.SelectedServiceKey = Constants.ServiceKeyMap[answer].Key;
                    Constants.SelectedServiceRegion = Constants.ServiceKeyMap[answer].Region;
                }
                //currently stopped, click to start
                await Task.Run( ()=>speechService.ListenContinuouslyAsync(gamePlayer));
                //RecordButton.Text = "Stop Record";
            }
            else
            {
                //currently started, click to stop
                App.LogService.LogInfo($"stop recording");
                speechService.SetStopFlag();
                //RecordButton.Text = "Start Record";
            }
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete training record", "Are you sure to delete?", "Yes", "No");
            if (answer)
            {
                var gamePlayer = (GamePlayer)BindingContext;
                var game = gamePlayer.ExportGame();
                App.LogService.LogInfo($"Delete record {game.ID}");
                await App.Database.DeleteNoteAsync(game);

                //gamePlayer.StopListen();
                speechService.SetStopFlag();
                this.RecordButton.Text = "Start Record";
                // Navigate backwards
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnHit(object sender, EventArgs e)
        {
            await gamePlayer.Hit();
        }

        private async void OnMiss(object sender, EventArgs e)
        {
            await gamePlayer.Miss();
        }

        protected override void OnDisappearing()
        {
            //gamePlayer.StopListen();
            speechService.SetStopFlag();
            //this.RecordButton.Text = "Start Record"; //stop record, change the button text
            base.OnDisappearing();
        }
    }
}