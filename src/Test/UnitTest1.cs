using System;
using NUnit.Framework;
using EnumCacheSpace;

namespace Test
{
    enum TestEnum
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 4
    }

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var array = EnumCache.GetUnderlyingValues<TestEnum, int>();
            foreach (var item in array)
            {
                Console.WriteLine(item);
            }
        }

        [Test]
        public void Test2()
        {

        }
    }
}