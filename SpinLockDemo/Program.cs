using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpinLockDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SpinLock slock = new SpinLock(false);
            long sum1 = 0;
            long sum2 = 0;
            Parallel.For(0, 10000, i => { sum1 += i; });

            Parallel.For(0, 10000, i =>
            {
                bool lockTaken = false;
                try
                {
                    slock.Enter(ref lockTaken);
                    sum2 += i;
                }
                finally
                {
                    if (lockTaken)
                        slock.Exit(false);
                }
            });

            Console.WriteLine("Num1的值为:{0}", sum1);
            Console.WriteLine("Num2的值为:{0}", sum2);


        }
    }
}