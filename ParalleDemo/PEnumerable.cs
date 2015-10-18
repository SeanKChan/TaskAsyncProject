using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParalleDemo
{
   public class PEnumerable
    {
       public static void ListWithParallel()
       {
           List<int> list = new List<int>();
           Parallel.For(0, 10000, item =>
           {
               list.Add(item);
           });
           Console.WriteLine("List's count is {0}",list.Count);//由此可知list是非线程安全集合，也就是说所有线程都可以修改它的值
       }

       public static void ConCurrentBagWithParallel()
       {
           ConcurrentBag<int> list = new ConcurrentBag<int>();
           Parallel.For(0, 10000, item =>
           {
               list.Add(item);
           });

           Console.WriteLine("Concurrent's count is {0}",list.Count);

           int n = 0;
           foreach (var i in list)
           {
               if (n > 10) break;
               n ++;
               Console.WriteLine("List[{0}] = {1}",n,i);
           }
       }

       public static void TestPlinq()
       {
           Stopwatch sw = new Stopwatch();
           List<Custom> customs = new List<Custom>();
           for (int i = 0; i < 2000000; i++)
           {
               customs.Add(new Custom() { Name = "Jack", Age = 21, Address = "NewYork" });
               customs.Add(new Custom() { Name = "Jime", Age = 26, Address = "China" });
               customs.Add(new Custom() { Name = "Tina", Age = 29, Address = "ShangHai" });
               customs.Add(new Custom() { Name = "Luo", Age = 30, Address = "Beijing" });
               customs.Add(new Custom() { Name = "Wang", Age = 60, Address = "Guangdong" });
               customs.Add(new Custom() { Name = "Feng", Age = 25, Address = "YunNan" });
           }

           sw.Start();
           var result = customs.Where<Custom>(c => c.Age > 26).ToList();
           sw.Stop();
           Console.WriteLine("Linq time is {0}.", sw.ElapsedMilliseconds);

           sw.Restart();
           sw.Start();
           var result2 = customs.AsParallel().Where<Custom>(c => c.Age > 26).ToList();
           sw.Stop();
           Console.WriteLine("Parallel Linq time is {0}.", sw.ElapsedMilliseconds);
       }


       public static void OrderByTest()
       {
           Stopwatch stopWatch = new Stopwatch();
           List<Custom> customs = new List<Custom>();
           for (int i = 0; i < 2000000; i++)
           {
               customs.Add(new Custom() { Name = "Jack", Age = 21, Address = "NewYork" });
               customs.Add(new Custom() { Name = "Jime", Age = 26, Address = "China" });
               customs.Add(new Custom() { Name = "Tina", Age = 29, Address = "ShangHai" });
               customs.Add(new Custom() { Name = "Luo", Age = 30, Address = "Beijing" });
               customs.Add(new Custom() { Name = "Wang", Age = 60, Address = "Guangdong" });
               customs.Add(new Custom() { Name = "Feng", Age = 25, Address = "YunNan" });
           }

           stopWatch.Restart();
           var groupByAge = customs.GroupBy(item => item.Age).ToList();
           foreach (var item in groupByAge)
           {
               Console.WriteLine("Age={0},count = {1}", item.Key, item.Count());
           }
           stopWatch.Stop();

           Console.WriteLine("Linq group by time is: " + stopWatch.ElapsedMilliseconds);


           stopWatch.Restart();
           var lookupList = customs.ToLookup(i => i.Age);
           foreach (var item in lookupList)
           {
               Console.WriteLine("LookUP:Age={0},count = {1}", item.Key, item.Count());
           }
           stopWatch.Stop();
           Console.WriteLine("LookUp group by time is: " + stopWatch.ElapsedMilliseconds);
       }
    }

    public class Custom
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int  Age { get; set; }
    }
}
