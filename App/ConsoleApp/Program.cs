using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {


        static void Main(string[] args)
        {
            string currenctDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.Write(currenctDir);
            File.WriteAllText(currenctDir + "connstr.txt", "hello");
            Console.ReadKey();
        }
    }
}
