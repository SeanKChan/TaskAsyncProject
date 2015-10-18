using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AwaiterDemo
{
    public class MyAwaiter : INotifyCompletion
    {
        public bool isCompleted
        {
            get { return false; }
        }

        private int result;

        public void OnCompleted(Action continuation)
        {
            Console.WriteLine("OnCompleted");
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(1000);
                this.result = 300;
                continuation();
            });
        }

        public int GetResult()
        {
            Console.WriteLine("GetResult");
         
            return this.result;
        }
    }

    internal static class Extend
    {
        public static MyAwaiter GetWatier(this int i)
        {
            return new MyAwaiter();
        }
    }
}