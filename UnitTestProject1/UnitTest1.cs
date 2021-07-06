using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Simplex;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            vvoddannyh vz = new vvoddannyh();
            vz.simplexBol();
            Assert.AreEqual(vz.table_result[vz.table_result.GetLength(0) - 1, 0] , -9, 111111);
        }
        [TestMethod]
        public void TestMethod2()
        {
            vvoddannyh vz = new vvoddannyh();
            vz.simplexBol();
            Assert.AreEqual(vz.table_result[vz.table_result.GetLength(0) - 1, 0] *-1, 9, 111111);
        }
        [TestMethod]
        public void TestMethod3()
        {
            vvoddannyh vz = new vvoddannyh();
            vz.simplexBol();
            Assert.AreEqual(vz.table_result[2,3], 0,6666667);
        }
        [TestMethod]
        public void TestMethod4()
        {
            vvoddannyh vz = new vvoddannyh();
            vz.simplexBol();
            Assert.AreEqual(vz.table_result[1, 0], 0,8888888);
        }
    }
}
