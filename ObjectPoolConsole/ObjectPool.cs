using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectPoolConsole
{
    public class ObjectPool<T>
    {
        private ConcurrentBag<T> _objects;
        private Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            if (objectGenerator == null) throw new ArgumentException("objectGenerator is null");
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
        }

        public T GetObject()
        {
            T item;
            if (_objects.TryTake(out item)) return item;//如果在对象池中有对象，则直接拿出来使用
            return _objectGenerator();//反之创建一个新对象
        }

        public void PutObject(T item)
        {
            _objects.Add(item);//将对象放到对象池中
        }
    }

    public class MyClass
    {
        public int[] Nums { get; set; }

        /// <summary>
        /// 求数组中某值平方根
        /// </summary>
        /// <param name="i">对应的索引</param>
        /// <returns></returns>
        public double GetValue(long i)
        {
            return Math.Sqrt(Nums[i]);
        }

        public MyClass ()
        {
            Nums = new int[1000000];
            Random random = new Random();
            for (int i = 0; i < Nums.Length; i++)
            {
                Nums[i] = random.Next();
            }
        }

    }
}