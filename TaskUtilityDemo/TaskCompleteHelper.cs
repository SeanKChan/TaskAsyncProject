using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TaskUtilityDemo
{
    /// <summary>
    /// 任务完成时处理 类
    /// </summary>
    public class TaskCompleteHelper
    {
        public static async Task<int> DelayAndReturnAsync(int val)
        {
            await Task.Delay(TimeSpan.FromSeconds(val));
            return val;
        }

        public static async Task AwaitAndProcessAync(Task<int> task)
        {
            var result = await task;
            Trace.WriteLine(result);
        }

        public static async Task ProcessTaskAsync()
        {
            Task<int> task1 = DelayAndReturnAsync(1);
            Task<int> task2 = DelayAndReturnAsync(2);
            Task<int> task3 = DelayAndReturnAsync(4);
            Task<int> task4 = DelayAndReturnAsync(3);

            var tasks = new[] {task1, task2, task3, task4};

            var processingTasks = (from t in tasks
                select AwaitAndProcessAync(t)).ToArray();

            await Task.WhenAll(processingTasks);
        }
    }
}