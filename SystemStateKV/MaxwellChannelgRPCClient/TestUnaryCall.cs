using System.Diagnostics;
using Grpc.Net.Client;
using LoggerLibrary.core;
using SystemStateKV;

namespace MaxwellChannelgRPCClient;

public class TestUnaryCall
{
    public void Test(int valueSize, int streamCount)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions(){MaxReceiveMessageSize=1000_000_000});
        var client = new SystemStateKVService.SystemStateKVServiceClient(channel);


        Logger.Info($"gRPC client created, query value count = {streamCount}");

        try
        {
            Random random = new Random();
            Stopwatch sw = Stopwatch.StartNew();
            int seq = 0;
            while (seq < streamCount)
            {
                var result = client.UpdateKeyValue(new KVRequest() { Key = seq.ToString(), Value = random.Next().ToString() });
                seq++;
                //Logger.Info($"{seq} msg received {result}");
            }

            sw.Stop();
            Logger.Info($"query {seq} values cost {sw.ElapsedMilliseconds} ms");
        }
        catch (Exception e)
        {
            Logger.Info(e.ToString());
        }
    }
}