using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TaskSchedulerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
//            Test();

            Test2();
        }

        #region coding

        static ActionBlock<int> readerAB1;
        static ActionBlock<int> readerAB2;
        static ActionBlock<int> readerAB3;
        static ActionBlock<int> writeAB1;

        static BroadcastBlock<int> bb = new BroadcastBlock<int>((i) => i);

        static void Test()
        {
            ConcurrentExclusiveSchedulerPair pair = new ConcurrentExclusiveSchedulerPair();

            readerAB1 = new ActionBlock<int>((i) =>
            {
                Console.WriteLine(i + "  ReaderAB1 begin handing." + " Excute Time:" + DateTime.Now +
                                  " currentThreadId : " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(500);
            }, new ExecutionDataflowBlockOptions() {TaskScheduler = pair.ConcurrentScheduler});

            readerAB2 = new ActionBlock<int>((i) =>
            {
                Console.WriteLine(i + "  ReaderAB2 begin handing." + " Excute Time:" + DateTime.Now +
                                  " currentThreadId : " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(500);
            }, new ExecutionDataflowBlockOptions() {TaskScheduler = pair.ConcurrentScheduler});

            readerAB3 = new ActionBlock<int>((i) =>
            {
                Console.WriteLine(i + "  ReaderAB3 begin handing." + " Excute Time:" + DateTime.Now +
                                  " currentThreadId : " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(500);
            }, new ExecutionDataflowBlockOptions() {TaskScheduler = pair.ConcurrentScheduler});

            writeAB1 = new ActionBlock<int>((i) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(i + "  writeAB1 begin handing." + " Excute Time:" + DateTime.Now +
                                  " currentThreadId : " + Thread.CurrentThread.ManagedThreadId);
                Console.ResetColor();
                Thread.Sleep(3000);
            }, new ExecutionDataflowBlockOptions() {TaskScheduler = pair.ExclusiveScheduler});

            bb.LinkTo(readerAB1);
            bb.LinkTo(readerAB2);
            bb.LinkTo(readerAB3);

            var task1 = Task.Run(() =>
            {
                while (true)
                {
                    bb.Post(1);
                    Thread.Sleep(1000);
                }
            });

            var task2 = Task.Run(() =>
            {
                while (true)
                {
                    writeAB1.Post(1);
                    Thread.Sleep(6000);
                }
            });

            Task.WaitAll(task1, task2);
        }

        #endregion

        #region  TDF中的负载均衡

        static BufferBlock<int> bufferB = new BufferBlock<int>();

        static ActionBlock<int> ab1 = new ActionBlock<int>((i) =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("ab1 handle data" + i + " Execute Time:" + DateTime.Now);
        },new ExecutionDataflowBlockOptions(){BoundedCapacity = 2});

        static ActionBlock<int> ab2 = new ActionBlock<int>((i) =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("ab2 handle data" + i + " Execute Time:" + DateTime.Now);
        },new ExecutionDataflowBlockOptions(){BoundedCapacity = 2});

        static ActionBlock<int> ab3 = new ActionBlock<int>((i) =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("ab3 handle data" + i + " Execute Time:" + DateTime.Now);
        }, new ExecutionDataflowBlockOptions() { BoundedCapacity = 2 });

        static void Test2()
        {
            bufferB.LinkTo(ab1);
            bufferB.LinkTo(ab2);
            bufferB.LinkTo(ab3);

            for (int i = 0; i < 10; i++)
            {
                bufferB.Post(i);
            }

            bufferB.Complete();
            bufferB.Completion.Wait();

        }

        #endregion
    }
}