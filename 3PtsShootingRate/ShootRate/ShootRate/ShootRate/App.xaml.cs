using System;
using System.IO;
//using GalaSoft.MvvmLight.Ioc;
using ShootRate.Data;
using ShootRate.Interface;
using ShootRate.Service;
using ShootRate.ViewModel;
using Xamarin.Forms;

namespace ShootRate
{
    public partial class App : Application
    {
        static NoteDatabase database;
        static ISpeechService speechService;
        
        // Create the database connection as a singleton.
        public static NoteDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return database;
            }
        }

        public static ILogService LogService { get; } = new LogService();

        public static ISpeechService SpeechService { get => speechService; set => speechService = value; }

        public App()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton<IShootResultListener>(new ShootResultListener());
            DependencyService.RegisterSingleton<IGamePlayer>(new GamePlayer());
            //LogService.Initialize();

            MainPage = new AppShell();
        }

        public void InitLogFile(string path)
        {
            LogService.Initialize(path);
            LogFileService.Initialize(path);
        }

        public void Stop()
        {
            LogService.Stop();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}