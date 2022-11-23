using ShootRate.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShootRate.Service
{
    public class LogFileService
    {
        private static string _path;

        public static void Initialize(string path)
        {
            _path = path;
        }

        internal static IEnumerable LoadAllLogFiles()
        {
            var files = System.IO.Directory.GetFiles(_path);
            return files
                .Select(x => new LogFile(){Path = x})
                .Where(y=>y.Size != "0 bytes")
                .OrderByDescending(z => z.Name);
        }

        internal static void Delete(string path)
        {
            System.IO.File.Delete(path);
        }
    }
}
