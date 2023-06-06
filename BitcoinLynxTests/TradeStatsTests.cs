using BitcoinLynx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynxTests
{
    [TestClass]
    public class TradeStatsTests
    {
        TradeStats ts;
        List<Transaction> list;

        [TestInitialize]
        public void setUp()
        {
            Transaction t1 = new Transaction(price: 0.5, amount: 10);
            Transaction t2 = new Transaction(price: 1.5, amount: 10);

            list = new();
            list.Add(t1);
            list.Add(t2);

            ts = new TradeStats(list);
            ts.calculateStats();
        }

        [TestMethod]
        public void testVolume()
        {
            Assert.AreEqual(20, ts.volume, "Volume not calculated correctly");
        }

        [TestMethod]
        public void testVwap()
        {
            Assert.AreEqual(1, ts.vwap, "Vwap not calculated correctly");
        }

        [TestMethod]
        public void testZeroVolumeAndVwap()
        {
            TradeStats emptyStats = new TradeStats(new List<Transaction>());
            emptyStats.calculateStats();
            Assert.AreEqual(0, emptyStats.vwap, "Vwap not calculated correctly");
            Assert.AreEqual(0, emptyStats.volume, "Volume not calculated correctly");
        }
    }
}
