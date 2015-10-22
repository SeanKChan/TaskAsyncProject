using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /* var p = Task.Factory.StartNew(Producer);
            var c = Task.Factory.StartNew(Consumer);
            Task.WaitAll(p, c);*/

            TestSync();
        }

        #region bufferBlock

        private static BufferBlock<int> m_buffer = new BufferBlock<int>();

        /// <summary>
        /// 生产者
        /// </summary>
        private static void Producer()
        {
            for (int i = 0; i < 4; i++)
            {
                m_buffer.Post(i);
            }
        }

        private static void Consumer()
        {
            for (int i = 0; i < 4; i++)
            {
                int item = m_buffer.Receive();
                Console.WriteLine(item);
            }
        }

        #endregion

        #region AcionBlock

        public static ActionBlock<int> abSync = new ActionBlock<int>(async (i) =>
        {
            Thread.Sleep(1000);
//            await Task.Delay(1000);
            Console.WriteLine(i + " ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
        }
            /*, new ExecutionDataflowBlockOptions() {MaxDegreeOfParallelism = 3}*/); //并行处理，最多可以3个并行处理

        public static void TestSync()
        {
            for (int i = 0; i < 10; i++)
            {
                abSync.Post(i);
            }

            abSync.Complete(); //让ActionBlock停止接受数据
            Console.WriteLine("Post finished");
            abSync.Completion.Wait(); //ActionBlock处理完所有数据会执行的任务
            Console.WriteLine("Process finished");
        }

        #endregion
    }
}