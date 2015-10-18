using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParalleDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
//            Class1.ParalleInvokeMethod();
            // Class1.PrallelForMethod();

            /*  Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.For(0, 10000, item =>
            {
               
                Console.Write(item + "\t");
            });*/
            //Class1.ParallelBreak();
//            Class1.ParallelException();
//            PEnumerable.ListWithParallel();
            PEnumerable.ConCurrentBagWithParallel();
//            PEnumerable.TestPlinq();
//            PEnumerable.OrderByTest();
        }
    }
}