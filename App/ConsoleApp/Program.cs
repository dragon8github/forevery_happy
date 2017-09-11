using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //int aa = TestLambda(str => {
            //    Console.WriteLine(str);
            //    return 1234;
            //});


            int aa = TestLambda(fuck => {
                Console.WriteLine(fuck);
            });

            Console.Write(aa);

            Console.ReadLine();
        }


        //public static int TestLambda(Func<string, int> action)
        //{
        //    // 调用这个函数，并且传递一个值
        //    return action("Hello World");
        //}

        public static int TestLambda(Action<string> action)
        {
            action("fuck");
            return 123;
        }
    }
}
