using ShootRate.Models;
using ShootRate.Service;
using System;
using System.ComponentModel;

namespace ShootRate.ViewModel
{
    internal class LogFileEntryViewModel : INotifyPropertyChanged
    {
        private string content;
        private string name;
        private LogFile _file;

        public LogFileEntryViewModel()
        {
            _file = new LogFile();
        }

        internal void Load(string path)
        {
            _file.Path = path;
            Name = _file.Name;
            Content = _file.Content;
        }

        public string Name { get { return name; } set { name = value; OnPropertyChanged(nameof(name)); } }
        public string Content { get { return content; } set { content = value; OnPropertyChanged(nameof(Content)); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void Delete()
        {
            try
            {
                LogFileService.Delete(_file.Path);
            }
            catch (Exception ex)
            {
                App.LogService.LogError($"Delete log file {_file.Name} failed");
            }
        }
    }
}