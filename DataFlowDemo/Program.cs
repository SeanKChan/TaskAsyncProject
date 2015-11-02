using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

//            TestSync();

//            TestDownloadHtml();

//            TestSync2();

//            TestSync3();

//            TestSync4();

//            TestSync5();
//            Test();

//            TestSync6();

//            TestSync7();

            TestSync8();

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
//            Thread.Sleep(1000);
            await Task.Delay(1000);
            Console.WriteLine(i + " ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
        }
            , new ExecutionDataflowBlockOptions() {MaxDegreeOfParallelism = 3}); //并行处理，最多可以3个并行处理

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

        #region TransformBlock

        public static TransformBlock<string, string> tbUrl = new TransformBlock<string, string>((url) =>
        {
            WebClient client = new WebClient();
            return client.DownloadString(url);
        });

        public static void TestDownloadHtml()
        {
            tbUrl.Post("http://www.baidu.com");
            tbUrl.Post("http://www.sina.com");

            for (var i = 0; i < 3; i++)
            {
                string result = tbUrl.Receive();
                Console.WriteLine(result);
            }
            tbUrl.Complete();
            Console.WriteLine("Post Finished");
            tbUrl.Completion.Wait();
            Console.WriteLine("Proccess Finished");
        }

        #endregion

        #region TranformManyBlock

        public static TransformManyBlock<int, int> tmb =
            new TransformManyBlock<int, int>((i) => new int[] {i, i + 1});

        public static ActionBlock<int> ab = new ActionBlock<int>((i) => Console.WriteLine(i));

        public static void TestSync2()
        {
            tmb.LinkTo(ab);

            for (int i = 0; i < 4; i++)
            {
                tmb.Post(i);
            }
            tmb.Complete();
            Console.WriteLine("Post Finished");
            tmb.Completion.Wait();
            Console.WriteLine("Process Finished");
        }

        #endregion

        #region BroadcastBlock

        private static BroadcastBlock<int> bb = new BroadcastBlock<int>((i) => { return i; });

        private static ActionBlock<int> displayBlock =
            new ActionBlock<int>((i) => { Console.WriteLine("Display " + i); });

        private static ActionBlock<int> saveBlock = new ActionBlock<int>((i) => { Console.WriteLine("Save " + i); });
        private static ActionBlock<int> sendBlock = new ActionBlock<int>((i) => { Console.WriteLine("Send " + i); });

        public static void TestSync3()
        {
            bb.LinkTo(displayBlock);
            bb.LinkTo(saveBlock);
            bb.LinkTo(sendBlock);

            for (int i = 0; i < 4; i++)
            {
                bb.Post(i);
            }
            //由此可知Post之后再添加linkToBlock的话，那些Block只会收到最后一条数据
            /*bb.LinkTo(displayBlock);
            bb.LinkTo(saveBlock);
            bb.LinkTo(sendBlock);*/


            bb.Complete();
            Console.WriteLine("Post Finished");
            bb.Completion.Wait();
            Console.WriteLine("Process Finished");
            Console.WriteLine("Recive:" + bb.Receive());
        }

        #endregion

        #region WriteOnceBlock

        private static WriteOnceBlock<int> wb = new WriteOnceBlock<int>((i) => { return i; });

        private static void TestSync4()
        {
            wb.LinkTo(displayBlock);
            wb.LinkTo(saveBlock);
            wb.LinkTo(sendBlock);

            for (int i = 0; i < 4; i++)
            {
                wb.Post(i);
            }

            wb.Complete();
            Console.WriteLine("Post Finished");
            wb.Completion.Wait();
            Console.WriteLine("Process Finished");
            Console.WriteLine("Recive:" + wb.Receive());
        }

        #endregion

        #region BatchBlock

        private static BatchBlock<int> batchB = new BatchBlock<int>(3);

        private static ActionBlock<int[]> ab1 = new ActionBlock<int[]>((i) =>
        {
            string s = i.Aggregate(string.Empty, (current, m) => current + (m + " "));
            Console.WriteLine(s);
        });

        private static void TestSync5()
        {
            batchB.LinkTo(ab1);
            for (int i = 0; i < 10; i++)
            {
                batchB.Post(i);
            }

            batchB.Complete();
            Console.WriteLine("Post Finished");
            batchB.Completion.Wait();
            Console.WriteLine("Process Finished");
//            Console.WriteLine(new Exception().ToString());
        }

        #endregion

        #region JoinBlock

        private static JoinBlock<int, string> joinB = new JoinBlock<int, string>();

        private static ActionBlock<Tuple<int, string>> ab2 =
            new ActionBlock<Tuple<int, string>>((i) => { Console.WriteLine(i.Item1 + "   " + i.Item2); });

        public static void TestSync6()
        {
            joinB.LinkTo(ab2);

            for (int i = 0; i < 5; i++)
            {
                joinB.Target1.Post(i);
            }

            for (int i = 5; i > 0; i--)
            {
                Thread.Sleep(1000);
                joinB.Target2.Post(i.ToString());
            }

            joinB.Complete();
            Console.WriteLine("Post Finished");
            //joinB.Completion.Wait();
//            Console.WriteLine("Process Finished");
        }

        #endregion

        #region BatchedJoinBlock

        private static BatchedJoinBlock<int, string> bjb = new BatchedJoinBlock<int, string>(3);

        private static ActionBlock<Tuple<IList<int>, IList<string>>> ab3 =
            new ActionBlock<Tuple<IList<int>, IList<string>>>((i) =>
            {
                Console.WriteLine("-----------------------------");
                foreach (int m in i.Item1)
                {
                    Console.WriteLine(m);
                }
                ;

                foreach (string s in i.Item2)
                {
                    Console.WriteLine(s);
                }
                ;
            });

        public static void TestSync7()
        {
            bjb.LinkTo(ab3);

            for (int i = 0; i < 5; i++)
            {
                bjb.Target1.Post(i);
            }

            for (int i = 5; i > 0; i--)
            {
                bjb.Target2.Post(i.ToString());
            }

            Console.WriteLine("Finished post");

            bjb.Receive();
        }

        #endregion


        #region TestSync8

        private static ActionBlock<int> ab4 = new ActionBlock<int>((i) =>
        {
            Thread.Sleep(1000);
            Console.WriteLine(i + " ThreadId : " + Thread.CurrentThread.ManagedThreadId + " Excute Time:" + DateTime.Now);
        });

        private static TransformBlock<int, int> tbSync = new TransformBlock<int, int>((i) => i*2);

        public static void TestSync8()
        {
            tbSync.LinkTo(ab4);

            for (int i = 0; i < 10; i++)
            {
                tbSync.Post(i);
            }
            for (int i = 9; i >= 0; i--)
            {
                Console.WriteLine(tbSync.Receive());
            }

            tbSync.Complete();
            Console.WriteLine("Post finished");

            tbSync.Completion.Wait();
            Console.WriteLine("TransformBlock process finished");

         

        }

        #endregion

        private static void Test()
        {
            int[] arr = {1, 2, 3, 4, 5, 6, 7};
            string s = string.Empty;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            s = arr.Aggregate(string.Empty, (current, m) =>
            {
                Console.WriteLine(" ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
                return current + (m + " ");
            });

            /* foreach (var  item in arr)
            {
                Console.WriteLine(" ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
                s += item + "  ";
            }*/
            sw.Stop();

            Console.WriteLine("Process Costs Time:" + sw.ElapsedMilliseconds);
            Console.WriteLine(" ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
            Console.WriteLine(s);
        }
    }
}