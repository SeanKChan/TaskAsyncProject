using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public class TaskHelper
    {
        private const int MAX_FILE_SIZE = 14000000;

        public static Task<string> GetFileStringAsync(string path)
        {
            FileInfo fi = new FileInfo(path);
            byte[] data = null;
            data = new byte[fi.Length];


            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, data.Length, true);

            Task<int> task = Task<int>.Factory.FromAsync(fs.BeginRead, fs.EndRead, data, 0, data.Length, null);

            return task.ContinueWith((antecedent) =>
            {
                fs.Close();
                if (antecedent.Result < 100)
                {
                    return "Data is too small to bother with.";
                }
                else
                {
                    if (antecedent.Result < data.Length)
                    {
                        Array.Resize(ref data, antecedent.Result);
                    }
                    return new UTF8Encoding().GetString(data);
                }
            });
        }
    }
}