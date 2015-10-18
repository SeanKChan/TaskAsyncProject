using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
            var task1 = new Task(() =>
            {
                Console.WriteLine("Hello,task");
            });
            task1.Start();

            var task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Hello,task started by taskFactory");
            });
            */

            #region 查看task的生命周期

            /*
                         var task3 = new Task(() =>
                        {
                            Console.WriteLine("Begin");
                            Thread.Sleep(2000);
                            Console.WriteLine("Finish");
                        });

                        Console.WriteLine("Before Status:" + task3.Status);

                        task3.Start();

                        Console.WriteLine("Run Status:" + task3.Status);

                        task3.Wait(); //等待task3任务执行完毕
                        Console.WriteLine("After Finish Status:" + task3.Status);
                        */

            #endregion

            #region  WaitAll

            /*

                        var task1 = new Task(() =>
                        {
                            Console.WriteLine("task1 Begin");
                            Thread.Sleep(2000);
                            Console.WriteLine("task1 Finish");
                        });
                        var task2 = new Task(() =>
                        {
                            Console.WriteLine("task2 Begin");
                            Thread.Sleep(3000);
                            Console.WriteLine("task2 Finish");
                        });
                        task1.Start();
                        task2.Start();
                        Task.WaitAll(task1, task2);
                        Console.WriteLine("All Task Finished");
            */

            #endregion

            #region WaitAny

            /*

                        var task1 = new Task(() =>
                        {
                            Console.WriteLine("task1 Begin");
                            Thread.Sleep(2000);
                            Console.WriteLine("task1 Finish");
                        });
                        var task2 = new Task(() =>
                        {
                            Console.WriteLine("task2 Begin");
                            Thread.Sleep(3000);
                            Console.WriteLine("task2 Finish");
                        });
                        task1.Start();
                        task2.Start();
                        Task.WaitAny(task1, task2);
                        Console.WriteLine("Any Task Finished");
            */

            #endregion

            #region ContinueWith

            /*var task1 = new Task(() =>
            {
                Console.WriteLine("task1 Begin");
                Thread.Sleep(3000);
                Console.WriteLine("task1 Finished");
            });

            var task2 = new Task(() =>
            {
                Console.WriteLine("task2 Begin");
                Thread.Sleep(5000);
                Console.WriteLine("task2 Finished");
            });

            task1.Start();
            task2.Start();

            var result = task1.ContinueWith<string>(task =>
            {
                Console.WriteLine("task1 Finished");
                return "This is Task Result";
            });

            Console.WriteLine(result.Result);*/

            #endregion

            #region ContinueWith 串行执行

            /* var sendFeedBackTask = Task.Factory.StartNew(() => { Console.WriteLine("Get Some Data"); })
                .ContinueWith<bool>(
                    s => true).ContinueWith<string>(s => s.Result ? "Well Done" : "Error Done");
            Console.WriteLine(sendFeedBackTask.Result);

            Console.Read();*/

            #endregion

            #region Task的Cancel

            /*

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Abort mission success");
                        return;
                    }
                    Console.WriteLine("task"+i+" running");
                }
            }, token);

            token.Register(() => { Console.WriteLine("Canceled"); });
            Console.WriteLine("Press  enter to cancel task ....");
            Console.ReadKey();
            tokenSource.Cancel();
*/

            #endregion

            #region  task嵌套（无父子关系嵌套）

            /*  var pTask = Task.Factory.StartNew(() =>
            {
                var cTask = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(3000);
                    Console.WriteLine("Children Task is Finished ");
                });

                Console.WriteLine("Parent Task is Finished");
            });

            pTask.Wait();
            Console.WriteLine("Flag");
            Console.Read();*/

            #endregion

            #region task嵌套（有父子关系）

            /*

            var pTask = Task.Factory.StartNew(() =>
            {
                var cTask = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(3000);
                    Console.WriteLine("Children Task is Finished ");
                },TaskCreationOptions.AttachedToParent);

                Console.WriteLine("Parent Task is Finished");
            });

            pTask.Wait();
            Console.WriteLine("Flag");
            Console.Read();
*/

            #endregion

            #region　多任务协作
/*
            Task.Factory.StartNew(() =>
            {
                var task1 = Task.Factory.StartNew<int>(() =>
                {
                    Console.WriteLine("task1 running...");
                    return 1;
                });
                task1.Wait();
                var task3 = Task.Factory.StartNew<int>(() =>
                {
                    Console.WriteLine("task3 running");
                    return task1.Result + 3;
                });

                var task4 = Task.Factory.StartNew<int>(() =>
                {
                    Console.WriteLine("task2 running...");
                    return task1.Result + 2;
                }).ContinueWith<int>(task=>
                {
                    Console.WriteLine("task4 running...");
                    return task.Result + 4;
                });

                Task.WaitAll(task3,task4);

                //等待task3  task4完成
                var result = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Task Finished! The result is {0}",task3.Result + task4.Result);
                });
            });

            Console.Read();*/

            #endregion


            #region 异常处理
/*

            try
            {
                var pTask = Task.Factory.StartNew(() =>
                {
                    var cTask = Task.Factory.StartNew(() =>
                    {
                        System.Threading.Thread.Sleep(2000);
                        throw new Exception("cTask Error!");
                        Console.WriteLine("Childen task finished!");
                    });
                    throw new Exception("pTask Error!");
                    Console.WriteLine("Parent task finished!");
                });

                pTask.Wait();
            }
            catch (AggregateException ex)
            {
                foreach (Exception inner in ex.InnerExceptions)
                {
                    Console.WriteLine(inner.Message);
                }
            }
            Console.WriteLine("Flag");
            Console.Read();
*/

            #endregion

            #region 设置最大等待时间

           /* var taskArr = new Task[2];
            taskArr[0] = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("task 0 running...");
                while (true)
                {
                    Thread.Sleep(2000);
                }
                Console.WriteLine("task 0 finished");
            });
            taskArr[1] = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task 2 Start running...");
                Thread.Sleep(2000);
                Console.WriteLine("Task 2 Finished!");
            });

            Task.WaitAll(taskArr,5000);

            for (int i = 0; i < taskArr.Length; i++)
            {
                if (taskArr[i].Status != TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("task {0} Error!",i);
                }
            }

            Console.Read();*/

            #endregion


            Task<string> t = TaskHelper.GetFileStringAsync(@"F:\123.txt");

            try
            {
                Console.WriteLine(t.Result.Substring(0, 500));
            }
            catch (AggregateException ae)
            {
                Console.WriteLine(ae.InnerException.Message);
            }
        }
    }
}