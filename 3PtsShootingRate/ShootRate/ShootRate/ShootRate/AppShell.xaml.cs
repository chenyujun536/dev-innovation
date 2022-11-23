using ShootRate.Views;
using Xamarin.Forms;

namespace ShootRate
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GameEntryPage), typeof(GameEntryPage));
            Routing.RegisterRoute(nameof(LogFileEntryPage), typeof(LogFileEntryPage));
        }

    }
}
