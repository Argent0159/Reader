using System;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        Func<string, string> info = val => $@"({val}\s*?[:：]\s*?(\S+?))?\s*?";

        [TestMethod]
        public void RegexPattern()
        {
            var cd = Directory.GetCurrentDirectory();

            Console.WriteLine($"CurrentDirectory is {cd}");



        }
    }
}
