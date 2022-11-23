// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Reflection;
using LoggerLibrary.core;
using Newtonsoft.Json.Linq;


if (args == null || args.Length == 0)
{
    Logger.Info("wrong argument");
    return;
}


string testFolder = args[0];
var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
testFolder = Path.Combine(rootPath, testFolder);
if (!Directory.Exists(testFolder))
{
    Logger.Info("wrong argument");
    return;
}

JObject o1 = JObject.Parse(File.ReadAllText(Path.Combine(testFolder, "client_setting.json")));
int testCount = Convert.ToInt32(o1["test_count"]);
int testCaseInterval = Convert.ToInt32(o1["test_case_interval"]);
int valueSize = Convert.ToInt32(o1["value_size"]);
int channelCount = Convert.ToInt32(o1["channel_count"]);
int valueStreamCount = Convert.ToInt32(o1["value_stream_count"]);
int valuePublishInterval = Convert.ToInt32(o1["value_publish_interval"]);
var testapp = Convert.ToString(o1["test_app"]);
var testSvr = Convert.ToString(o1["test_svr"]);
var waitServer = Convert.ToInt32(o1["wait_server"]);
var waitClient = Convert.ToInt32(o1["wait_client"]);
var serverNoArgument = Convert.ToBoolean(o1["svr_no_argument"]);


Stopwatch sw = Stopwatch.StartNew();

Logger.Info($"Test started with \n test_svr={testSvr}, test_app={testapp} \n test_count={testCount},value_size={valueSize},stream_count={valueStreamCount}, interval={valuePublishInterval}, wait_client={waitClient}, channel_count={channelCount}, wait_server={waitServer}");

Process server = new Process();
server.StartInfo = new ProcessStartInfo()
{
    UseShellExecute = true,
    WorkingDirectory = Path.Combine(testFolder, "server"),
    FileName = testSvr,
    Arguments = serverNoArgument? "": $"-vs={valueSize} -sc={valueStreamCount} -sr={valuePublishInterval} -wc={waitClient} -cc={channelCount}"
};
server.Start();

Logger.Info($"test server starts, wait it for {waitServer} seconds");
Thread.Sleep(waitServer * 1000);
Logger.Info("test server started, will start client now");

List<Process> processList = new List<Process>();

for (int i = 0; i < testCount; i++)
{
    Process p = new Process();
    p.StartInfo = new ProcessStartInfo()
    {
        UseShellExecute = true,
        WorkingDirectory = Path.Combine(testFolder, "client"),
        FileName = testapp,
        Arguments = $"-vs={valueSize} -sc={valueStreamCount} -sr={valuePublishInterval} -cc={channelCount}",
    };
    p.Start();
    processList.Add(p);
    Logger.Info($"client {i} started");
    if(testCaseInterval > 0)
        Thread.Sleep(testCaseInterval);
}

Logger.Info("wait for all test client to exit");
await Task.WhenAll(processList.Select(x => x.WaitForExitAsync()));
Logger.Info("test client stopped");

server.Kill();

Logger.Info("test server stopped");

sw.Stop();
Logger.Info($"finished testing, using {sw.ElapsedMilliseconds} ms");