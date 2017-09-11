using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int aa = TestLambda(str => {
                Console.WriteLine(str);
                return 1234;
            });
            Console.WriteLine(aa + "fuck");
            Console.ReadLine();
        }


        public static int TestLambda(Func<string, int> action)
        {
            // 调用这个函数，并且传递一个值
            int a = action("Hello World");
            Console.WriteLine(a);
            return a;
        }

        public static void TestLambda(Action<string> action)
        {

        }
    }
}
