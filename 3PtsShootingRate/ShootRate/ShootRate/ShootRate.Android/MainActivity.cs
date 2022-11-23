
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Reflection;
using System.IO;
using System.Linq;
using Xamarin.Forms.Internals;

namespace ShootRate.Droid
{
    [Activity(Label = "ShootRate", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        App application;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            application = new App();
            var path = GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            application.InitLogFile(path.Path);
            LoadApplication(application);


            //string[] files = Directory.GetFiles(path.Path);
            //files.ForEach(f => System.Console.WriteLine(f));
        }

        protected override void OnStop()
        {
            base.OnStop();
            application?.Stop();

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}