using ShootRate.Interface;
using ShootRate.Models;
using ShootRate.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShootRate.ViewModel
{
    public class GamePlayer : IGamePlayer, INotifyPropertyChanged
    {
        private const int displayGroupSize = 10;
        private readonly ObservableCollection<string> _hits = new ObservableCollection<string>();
        public ObservableCollection<string> Hits => _hits;
        public int Tries { get; private set; }

        public Game ExportGame()
        {
            _game.Hits = HitsInStorageFormat();
            _game.Tries = Tries;
            _game.Percentage = Percentage;
            _game.Summary = Summary;
            return _game;
        }

        internal void InitWith(Game game)
        {
            App.LogService.LogInfo($"init game record {game.ID}");
            _game = game;
            var hitsInSeq = HitsInDisplayFormat(game.Hits);
            Hits.Clear();
            foreach (int item in hitsInSeq)
            {
                AddHit(item);
            }
            Tries = game.Tries;
            Summarize();
            _textToSpeech = RecognizeServiceBuilder.BuildTextForSpeech();
        }

        public async Task StartSession()
        {
            await _textToSpeech.SpeakAsync("started");
        }

        public async Task StopSession()
        {
            await _textToSpeech.SpeakAsync("stopped");
        }

        private void AddHit(int hitSeq)
        {
            int groupSeq = (hitSeq - 1) / displayGroupSize;

            while (groupSeq >= Hits.Count)
                Hits.Add(string.Empty);

            var groupHits = Hits.Last();

            groupHits = string.Join(",", groupHits, hitSeq).TrimStart(new[] { ',' });
            Hits.RemoveAt(Hits.Count - 1);
            Hits.Add(groupHits);
        }

        private void Summarize()
        {
            var hitCount = Hits.SelectMany(x => x.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).Count();
            Percentage = (Tries == 0) ? 0 : (hitCount * 100 / Tries);
            Summary = $"{hitCount} hits out of {Tries} tries, {Percentage}%";
        }

        private async Task ReportPercentage()
        {
            if (Tries % displayGroupSize == 0)
            {
                await _textToSpeech.SpeakAsync($"综合命中率 {Percentage}%");
            }
        }


        public async Task Hit()
        {
            await Task.Run(() =>
            {
                Tries += 1;
                AddHit(Tries);
                Summarize();
            });
            await Save();
            await _textToSpeech.SpeakAsync(Tries + " 命中");
            App.LogService.LogDebug(Tries + " 命中");
            await ReportPercentage();
        }

        public async Task Save()
        {
            var game = ExportGame();
            await App.Database.SaveNoteAsync(game);
        }

        public async Task Miss()
        {
            await Task.Run(() =>
            {
                Tries += 1;
                Summarize();
            });
            await Save();

            await _textToSpeech.SpeakAsync(Tries + " 偏出");
            App.LogService.LogDebug(Tries + " 偏出");
            await ReportPercentage();
        }

        private int[] HitsInDisplayFormat(string hits)
        {
            if (string.IsNullOrEmpty(hits))
                return new int[0];

            var hitSeq = hits.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return hitSeq.Select(x => Convert.ToInt32(x)).ToArray();
        }

        public string HitsInStorageFormat()
        {
            return string.Join(",", Hits.Select(x => x.ToString()));
        }

        private string _summary;
        private Game _game;

        public int Percentage { get; private set; }

        public string Summary
        {
            get => _summary;
            set { _summary = value; OnPropertyChanged(nameof(Summary)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Rollback()
        {
            await Task.Run(async () =>
            {

                await _textToSpeech.SpeakAsync("撤销 " + Tries);
                App.LogService.LogDebug("撤销 " + Tries);

                string hitCountToRemove = Tries.ToString();
                Tries -= 1;
                if (Tries <= 0)
                {
                    Tries = 0;
                    Hits.Clear();
                    Summarize();
                    await Save();
                    return;
                }

                if (Tries % displayGroupSize == 0)
                {
                    Hits.RemoveAt(Hits.Count - 1);
                    Summarize();
                    await Save();
                    return;
                }

                var groupHits = Hits.LastOrDefault();
                if (string.IsNullOrEmpty(groupHits) || !groupHits.Contains(hitCountToRemove))
                {
                    Summarize();
                    await Save();
                    return;
                }

                groupHits = groupHits.Remove(groupHits.IndexOf(hitCountToRemove)).TrimEnd(new[] { ',' });
                Hits.RemoveAt(Hits.Count - 1);
                Hits.Add(groupHits);
                Summarize();
                await Save();
            });
        }


        private string recordStatus="Record Stopped";
        private ITextToSpeech _textToSpeech;

        public string RecordStatus { get { return recordStatus; } set { recordStatus = value; OnPropertyChanged(nameof(RecordStatus)); } }

    }
}
