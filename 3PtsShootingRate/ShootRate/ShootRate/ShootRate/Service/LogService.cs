using System;
using System.IO;
using System.Threading.Tasks;

namespace ShootRate.Service
{
    public class LogService : ILogService
    {
        private StreamWriter writer;
        private DateTime lastLogTimeStamp = DateTime.Now;

        public void Stop()
        {
            writer?.Dispose();
            writer = null;
        }

        public void Initialize(string filePath)
        {
            //var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LogService)).Assembly;
            //Stream stream = assembly.GetManifestResourceStream("ShootRate.Service.NLog.config");            
            //LogManager.Configuration = new XmlLoggingConfiguration(XmlReader.Create(stream));
            //this.logger = LogManager.GetCurrentClassLogger();
            //for (int i = 0; i < 59; i++)
            //{
            //    System.Environment.SpecialFolder folderName = (System.Environment.SpecialFolder)i;
            //    string folderPath2 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //    if (Directory.Exists(folderPath2))
            //        Console.WriteLine($"{folderName} {folderPath2} exists");
            //    else
            //        Console.WriteLine($"{folderName} {folderPath2} doesn't exit");
            //}

            var folderPath = filePath; // Path.Combine(filePath, "ShootRate/logs");
            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var backingFile = Path.Combine(folderPath, $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.log");
            writer = File.CreateText(backingFile);
            writer.AutoFlush = true;
        }

        public void LogDebug(string message)
        {
            //this.logger.Info(message);
            LogInternal(message, "DEBUG");
        }

        private void LogInternal(string message, string level)
        {
            var now = DateTime.Now;
            var elapsed = (int)(now - lastLogTimeStamp).TotalSeconds;
            lastLogTimeStamp = now;
            while (true)
            {
                try 
                { 
                    writer.WriteLine($"{now:HH:mm:ss:fff} {elapsed} {level} {message}");
                    break;
                }
                catch (Exception e) 
                {
                }
            }
        }

        public void LogError(string message)
        {
            //this.logger.Error(message);
            LogInternal(message, "ERROR");
        }

        public void LogFatal(string message)
        {
            //this.logger.Fatal(message);
            LogInternal(message, "FATAL");
        }

        public void LogInfo(string message)
        {
            //this.logger.Info(message);
            LogInternal(message, "INFO");
        }

        public void LogWarning(string message)
        {
            //this.logger.Warn(message);
            LogInternal(message, "WARNING");
        }
    }

    public interface ILogService
    {
        void Stop();
        void Initialize(string path);
        void LogDebug(string message);
        void LogError(string message);
        void LogFatal(string message);
        void LogInfo(string message);
        void LogWarning(string message);
    }
}
