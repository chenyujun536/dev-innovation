using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsageReport;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private string imageFolder = "C:\\Temp";

        public Form1()
        {
            InitializeComponent();
            MakeDir();
        }

        private void MakeDir()
        {
            try
            {
                var path = Assembly.GetEntryAssembly()?.Location;
                var directory = (path == null) ? "C:\\Temp" : Path.GetDirectoryName(path);

                string subfolder = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";
                imageFolder = Path.Combine(directory, subfolder);
                Directory.CreateDirectory(imageFolder);
            }
            catch (Exception e)
            {
                Logger.Info(e.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {   
                int width = Screen.PrimaryScreen.Bounds.Width;
                int height = Screen.PrimaryScreen.Bounds.Height;
                //var size =Screen.PrimaryScreen.Bounds.Size;
                Bitmap captureBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Rectangle captureRectangle = Screen.PrimaryScreen.Bounds;
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0,
                    captureRectangle.Size);

                captureBitmap.Save(Path.Combine(imageFolder, $"snapshot_{DateTime.Now:HH-mm-ss}.jpg"), ImageFormat.Jpeg);
            }
            catch(Exception ex)
            {
                Logger.Info(ex.ToString());
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();

            var internalString = ConfigurationManager.AppSettings["SnapshotInterval"];
            if (Int32.TryParse(internalString, out int intervalNumber))
            {
                this.timer1.Interval = intervalNumber;
            }
        }
    }
}
