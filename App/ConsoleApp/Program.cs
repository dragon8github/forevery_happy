using System;
using System.Reflection;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var uuid = Guid.NewGuid().ToString();
            Console.WriteLine(uuid);

            Console.ReadLine();
        }
    }
}
