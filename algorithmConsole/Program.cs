using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace algorithmConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int count = 0;
            var arr = new int[] {2, 3, 1, 4, 6};
//            Sort(arr,ref count);
            EbullitionSorter(arr, ref count);
//            InsertSort(arr);
        }

        /// <summary>
        /// 选择排序
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="count"></param>
        private static void Sort(int[] arr, ref int count)
        {
            for (int i = 0; i < arr.Length - 1; i++) //外层 n-1
            {
                count ++;
                int min = i;
                for (int j = i + 1; j < arr.Length; j++) //内层也是n-1
                {
                    count ++;
                    if (arr[i] > arr[j])
                    {
                        min = j;
                        break;
                    }
                }
                int temp = arr[min];
                arr[min] = arr[i];
                arr[i] = temp;
            }
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(count.ToString());
        }

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="count"></param>
        private static void EbullitionSorter(int[] arr, ref int count)
        {
            int i, j, temp;
            bool flag = false;//交换标识
            for (i = 0; i < arr.Length - 1 && !flag; i++) //最多做n-1次
            {
                flag = false;
                count ++;
                for ( j = arr.Length - 2; j >= i; j--)
                {
                    if (arr[j] > arr[j+1])
                    {
                        temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                        flag = true;
                    }

                }
            }
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(count.ToString());
        }

        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="arr"></param>
        private static void InsertSort(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int temp = arr[i];
                int j = i;
                while ((j > 0) && (arr[j - 1] > temp))
                {
                    arr[j] = arr[j - 1];
                    --j;
                }
                arr[j] = temp;
              
            }
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }

        private static void test()
        {
            Hashtable ht = new Hashtable();
        }
    }
}