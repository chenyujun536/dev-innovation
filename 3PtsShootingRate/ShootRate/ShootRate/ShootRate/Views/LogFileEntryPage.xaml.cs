using ShootRate.Models;
using ShootRate.Service;
using ShootRate.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShootRate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(Path), nameof(Path))]
    public partial class LogFileEntryPage : ContentPage
    {
        public LogFileEntryPage()
        {
            InitializeComponent();
            BindingContext = _logFileViewModel;
        }

        public string Path
        {
            set
            {
                LoadLogFile(value);
            }
        }

        LogFileEntryViewModel _logFileViewModel = new LogFileEntryViewModel();

        private void LoadLogFile(string path)
        {
            _logFileViewModel.Load(path);
        }

        private async void OnEmailButtonClicked(object sender, EventArgs e)
        {
            //defaultActivityIndicator.IsVisible = true;
            defaultActivityIndicator.IsRunning = true;
            
            MailService service = new MailService();
            await service.Send($"log file {_logFileViewModel.Name}", _logFileViewModel.Content);
            //defaultActivityIndicator.IsVisible = false;
            defaultActivityIndicator.IsRunning = false;
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete Log File", $"Are you sure to delte log {_logFileViewModel.Name}", "Yes", "No");
            if (answer)
            {
                _logFileViewModel.Delete();
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}