// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using FASTER.core;
using LoggerLibrary.core;
using Microsoft.Extensions.Logging;


object lockObj = new object();

Console.WriteLine("Hello, World!");


using var settings = new FasterKVSettings<string, string>("C:\\temp.xml");
using var store = new FasterKV<string, string>(settings);
using var session = store.NewSession(new SimpleFunctions<string, string>((a, b) => a + b));

long Count = 10000;
Random random = new Random();
Logger.Info("faster kv starts");
Stopwatch sw = Stopwatch.StartNew();
Parallel.For(1, Count, i =>
{
    string key = i.ToString();
    string value1 = random.Next(1000).ToString();
    string output1 = "";
    session.Upsert(ref key, ref value1);
    session.Read(ref key, ref output1);
    output1 += $"_{key}";
    session.Upsert(ref key, ref output1);
    //LoggerLibrary.core.Logger.Debug(key.ToString());
});
sw.Stop();
Logger.Info($"faster kv write {Count} values use {sw.ElapsedMilliseconds} ms");

var dictionary = new Dictionary<string, string>();

sw.Restart();
Logger.Info("dictionary starts");
Parallel.For(1, Count, i=>
{
    string key = i.ToString();
    string val = random.Next(1000).ToString();
    lock (lockObj)
    {
        dictionary[key] = val;
        dictionary.TryGetValue(key, out val);
        val += $"_{key}";
        dictionary[key] = val;
        //LoggerLibrary.core.Logger.Debug(i.ToString());
    }
});
sw.Stop();
Logger.Info($"dictionary write {Count} values use {sw.ElapsedMilliseconds} ms");
