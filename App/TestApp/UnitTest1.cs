using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int a = 100;
            int b = 200;
            Console.WriteLine(123);
            Assert.AreEqual(b - a, 100);
        }
    }
}
