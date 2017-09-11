using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string Path = Directory.GetCurrentDirectory() + "/assets";
            if (!Directory.Exists(Path)) {
                Directory.CreateDirectory(Path);
            }
            string myTime = GetTimeStamp();
            Console.WriteLine("Hello World!" + Path);
            Console.WriteLine("Hello World!" + myTime);
            Console.ReadLine();
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
    }
}
