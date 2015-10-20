using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelDemo2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<int> list = new List<int>() {1, 2, 3, 4, 5, 6};
            int result = 0;


//            result = ParallelSum(list);
           
            result = ParallelSum2(list);
//            result = ParallelSum3(list);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 并行聚合 方式1（并不是最高效的实现方式）
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static int ParallelSum(IEnumerable<int> values)
        {
            //创建一个变量 互斥锁
            object mutex = new object();
            int result = 0;
            Parallel.ForEach(source: values, localInit: () => 0, body: (item, state, localValue) => localValue + item,
                localFinally:
                    localValue =>
                    {
                        lock (mutex)
                        {
                            result +=localValue;
                        }
                    });
            return result;
        }

        private static int ParallelSum2(IEnumerable<int> values)
        {
            return values.AsParallel().Sum();
        }

        private static int ParallelSum3(IEnumerable<int> values)
        {
            return values.AsParallel().Aggregate(seed: 0, func: (sum, item) => sum + item);
        }
    }
}