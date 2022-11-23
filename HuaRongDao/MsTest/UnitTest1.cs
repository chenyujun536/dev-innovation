using System;
using System.Collections.Generic;
using HuaRongDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int refN = 0;
            MergeSortAlg.MergeSorted(new List<int>() {4, 5}, new List<int>() {3, 6}, ref refN);
            Assert.AreEqual(2, refN);
        }
    }
}
