// See https://aka.ms/new-console-template for more information

using MaxwellChannelgRPCClient;

Console.WriteLine("test started");

//new TestUnaryCall().Test();

string channelOsdd = "GR";
int valueSize = 1;
int streamCount = 100;
int samplingRate = 20;

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

//await new TestServerStreamCall().TestAsync(channelOsdd, valueSize, streamCount, samplingRate);
new TestUnaryCall().Test(valueSize, streamCount);

Console.WriteLine("test finished");