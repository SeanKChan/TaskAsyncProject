using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace HttpTaskClient
{
    class Program
    {
        static void Main(string[] args)
        {

            int contentLength =  AccessTheWebAsync().Result;
            Console.WriteLine("\r\nLength of the downloaded string: {0}.\r\n", contentLength);
        }

        async static Task<int> AccessTheWebAsync()
        {
            HttpClient httpClient = new HttpClient();
            Task<string> getStringTask = httpClient.GetStringAsync("http://msdn.microsoft.com");

            DoIndependentWork();

            string urlContent = await getStringTask;

            return urlContent.Length;
        }


        private static void DoIndependentWork()
        {
            Console.WriteLine("Working......\r\n");
        }
    }
}
