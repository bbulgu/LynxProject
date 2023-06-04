using BitcoinLynx;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitcoinLynxTests
{
    [TestClass]
    public class UnitTest1
    {
        TradeData td;
        [TestInitialize]
        public void setUp()
        {
            Transaction t1 = new Transaction(price: 0.5, amount: 10);
            Transaction t2 = new Transaction(price: 1.5, amount: 10);

            List<Transaction> list = new();
            list.Add(t1);
            list.Add(t2);

            td = new TradeData();
            td.listOfTransactions = list;
            td.calculateStats();
        }

        [TestMethod]
        public void testVolume()
        {
            Assert.AreEqual(20, td.volume, 0.001, "Volume not calculated correctly");
        }

        [TestMethod]
        public void testVwap()
        {
            Assert.AreEqual(1, td.vwap, 0.001, "Vwap not calculated correctly");
        }
    }
}
