using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParalleDemo
{
    internal class Class1
    {
        private static readonly Stopwatch sw = new Stopwatch();

        public static void Run1()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Task 1 is cost 2 seconds");
        }

        public static void Run2()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 is cost 3 seconds");
        }

        public static void Run3()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Task 3 is cost 3 seconds");
            throw new Exception("Exception in Task 3");
        }

        public static void Run4()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Task 4 is cost 3 seconds");
            throw new Exception("Exception in Task 4");
        }

        public static void ParalleInvokeMethod()
        {
            sw.Start();
            Parallel.Invoke(Run1, Run2);
            sw.Stop();
            Console.WriteLine("Parallel run " + sw.ElapsedMilliseconds + "ms");
            sw.Restart();
            Run1();
            Run2();
            Console.WriteLine("Normal run " + sw.ElapsedMilliseconds + "ms");
        }

        public static void PrallelForMethod()
        {
            long num = 0;
            ConcurrentBag<long> bag = new ConcurrentBag<long>();
            var obj = new Object();
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 60000; j++)
                {
                    /* int sum = 0;
                    sum += i;*/
                    num ++;
                }
            }
            sw.Stop();
            Console.WriteLine("NormalFor run " + sw.ElapsedMilliseconds + "ms");

            sw.Reset();
            sw.Start();
            Parallel.For(0, 10000, item =>
            {
                for (int j = 0; j < 60000; j++)
                {
                    /* int sum = 0;
                    sum += item;*/
                    lock (obj)
                    {
                        num ++;
                    }
                }
            });
            sw.Stop();
            Console.WriteLine("ParallelFor run " + sw.ElapsedMilliseconds + " ms.");
        }

        public static void ParallelForeach()
        {
            sw.Start();
            List<int> list = new List<int>();
            list.Add(0);
            list.Add(1);
            list.Add(2);
            Parallel.ForEach(list, item => { Console.WriteLine("item"); });
            sw.Stop();
            Console.WriteLine("ParallelFor run " + sw.ElapsedMilliseconds + " ms.");
        }

        public static void ParallelBreak()
        {
            ConcurrentBag<long> bag = new ConcurrentBag<long>();
            sw.Start();
            Parallel.For(0, 10000, (i, state) =>
            {
                if (bag.Count == 300)
                {
                    state.Stop();
                    return;
                }
                bag.Add(i);
            });
            sw.Stop();
            Console.WriteLine("Bag count is " + bag.Count + ", " + sw.ElapsedMilliseconds);
        }


        public static void ParallelException()
        {
            sw.Start();
            try
            {
                Parallel.Invoke(Run3, Run4);
            }
            catch (AggregateException aex)
            {
                foreach (var ex in aex.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            sw.Stop();
            Console.WriteLine("Parallel run " + sw.ElapsedMilliseconds + " ms.");

            sw.Reset();
            sw.Start();
            try
            {
                Run3();
                Run4();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sw.Stop();
            Console.WriteLine("Normal run " + sw.ElapsedMilliseconds + " ms.");
        }
    }
}