using System;
using System.Collections.Generic;
using System.Text;

namespace ShootRate.Models
{
    public class LogFile
    {
        private string _path;

        public string Name { get; set; }
        public string Size { get; private set; }

        public string Content
        {
            get 
            {
                if(string.IsNullOrEmpty(_path))
                    return string.Empty;

                return System.IO.File.ReadAllText(_path);
            }

        }


        public string Path
        {
            set
            {
                Name = string.Empty;
                _path = value;
                if (string.IsNullOrEmpty(value))
                    { return; }

                Name = System.IO.Path.GetFileNameWithoutExtension(value);
                var length = new System.IO.FileInfo(_path).Length;

                if(length >1000)
                {
                    Size = $"{string.Format("{0:F}", (double)length / 1000)} KB";
                }
                else
                {
                    Size = $"{length} bytes";
                }
            }
            get
            {
                return _path;
            }
        }
    }
}
