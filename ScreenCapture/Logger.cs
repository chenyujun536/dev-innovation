using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace UsageReport
{
    public static class Logger
    {

        static Logger()
        {
            string logFile;
            try
            {
                var path = Assembly.GetEntryAssembly()?.Location;
                var directory = Path.GetDirectoryName(path);
                var fileName = Path.GetFileNameWithoutExtension(path);
                logFile = Path.Combine(directory, $"{fileName}_log_{DateTime.Now:HH-mm-ss.fff}.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                logFile = @"C:\\Temp\\sampletestlog.txt";
            }

            LogSw = new StreamWriter(logFile);
        }

        private static readonly StreamWriter LogSw; // = new StreamWriter(logFile);
        private static readonly object LockObject = new object();

        private static void LogLine(string line, bool debug)
        {
            lock (LockObject)
            {
                if (!debug)
                    Console.WriteLine(
                        $"[{DateTime.Now:HH-mm-ss.fff}] [Thread {Thread.CurrentThread.ManagedThreadId}] {line}");

                if (LogSw != null)
                {
                    LogSw?.WriteLine(
                        $"[{DateTime.Now:HH-mm-ss.fff}] [Thread {Thread.CurrentThread.ManagedThreadId}] {line}");
                    LogSw?.Flush();
                }
            }
        }

        public static void Info(string line)
        {
            LogLine(line, false);
        }

        public static void Debug(string line)
        {
            LogLine(line, true);
        }

    }
}