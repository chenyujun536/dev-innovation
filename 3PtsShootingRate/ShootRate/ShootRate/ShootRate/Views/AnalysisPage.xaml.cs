using ShootRate.Models;
using ShootRate.Service;
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
    public partial class AnalysisPage : ContentPage
    {
        public AnalysisPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = LogFileService.LoadAllLogFiles();
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to the NoteEntryPage, passing the ID as a query parameter.
                LogFile note = (LogFile)e.CurrentSelection.FirstOrDefault();

                //var answer = await DisplayAlert("Selected", $"you select {note.Name}", "Yes", "No");
                
                await Shell.Current.GoToAsync($"{nameof(LogFileEntryPage)}?{nameof(LogFileEntryPage.Path)}={note.Path}");
            }
        }
    }
}