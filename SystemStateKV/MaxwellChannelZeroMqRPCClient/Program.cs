using System;
using System.Diagnostics;
using System.Text;
using LoggerLibrary.core;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;

static class Program
{
    public static void Main(string[] args)
    {
        int valueSize = 1;
        int streamCount = 10000;
        int samplingRate = 0;

        foreach (var arg in args)
        {
            int value;
            try
            {
                value = Convert.ToInt32(arg.Split('=').Last());
            }
            catch
            {
                continue;
            }
            if (arg.StartsWith("-vs"))
            {
                valueSize = value;
            }
            else if (arg.StartsWith("-sc"))
            {
                streamCount = value;
            }
            else if (arg.StartsWith("-sr"))
            {
                samplingRate = value;
            }
        }

        Console.WriteLine("Connecting to ZeroMQ server…");
        using (var requester = new RequestSocket())
        {
            requester.Connect("tcp://localhost:5555");

            Stopwatch sw = Stopwatch.StartNew();
            Logger.Info($"start client query msg {streamCount}");
            Random random = new Random();
            for (int i = 0; i <streamCount; i++)
            {
                requester.SendFrame($"{i}={random.Next()}");
                var result = requester.ReceiveFrameString();
                //Logger.Debug(result);
                
                if(samplingRate >0)
                    Thread.Sleep(samplingRate);
            }
            sw.Stop();
            Logger.Info($"stop client. query msg {streamCount}, time {sw.ElapsedMilliseconds} ms");
        }
    }
}
