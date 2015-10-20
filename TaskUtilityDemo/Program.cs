using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtilityDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var task = TaskCompleteHelper.ProcessTaskAsync();
            task.Wait();
           
        }
    }
}