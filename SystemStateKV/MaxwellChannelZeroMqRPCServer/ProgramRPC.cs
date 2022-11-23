// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using System.Threading;
using LoggerLibrary.core;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json.Linq;

namespace MaxwellChannelZeroMqServer;

static class Program
{
    private static object _lockObj = new object();

    public static void Main(string[] args)
    {
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
            if (arg.StartsWith("-sc"))
            {
                streamCount = value;
            }
            else if (arg.StartsWith("-sr"))
            {
                samplingRate = value;
            }
        }

        int msgSeq = 0;
        using (var responder = new ResponseSocket())
        {
            responder.Bind("tcp://*:5555");
            Logger.Info($"publish ready, sc={streamCount}");

            while (true)
            {
                string str = responder.ReceiveFrameString();
                lock (_lockObj)
                {
                    string[] words = str.Split('=', StringSplitOptions.RemoveEmptyEntries);
                    int key = Convert.ToInt32(words[0]);
                    responder.SendFrame($"key {key} is updated");
                    msgSeq++;
                    //Logger.Info($"msg {msgSeq} published");
                    if (msgSeq >= streamCount*10)
                    {
                        Logger.Info("publish done");
                        break;
                    }
                }
            }
            Logger.Info($"msg {msgSeq} publish done");
        }
    }
}
