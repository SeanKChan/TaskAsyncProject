using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectPoolConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task task = new Task(() =>
            {
                if (Console.ReadKey().KeyChar == 'c' || Console.ReadKey().KeyChar == 'C')
                {
                    cts.Cancel();
                }
            });
            task.Start();

            ObjectPool<MyClass> pool = new ObjectPool<MyClass>(() => new MyClass());

            Parallel.For(0, 1000000, (i, loopstate) =>
            {
                MyClass mc = pool.GetObject();
                Console.CursorLeft = 0;
                Console.WriteLine("{0:####.####}", mc.GetValue(i));

                pool.PutObject(mc);
                if (cts.Token.IsCancellationRequested)
                {
                    loopstate.Stop();
                }
            });

            Console.WriteLine("Press the Enter key to exit.");
            Console.ReadLine();

        }
    }
}