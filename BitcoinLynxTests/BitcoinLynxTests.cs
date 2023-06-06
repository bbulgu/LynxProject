using BitcoinLynx;
using Moq;
namespace BitcoinLynxTests
{
    [TestClass]
    public class BitcoinLynxTests
    {
        TradeData td;
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
            Assert.AreEqual(20, ts.volume, 0.001, "Volume not calculated correctly");
        }

        [TestMethod]
        public void testVwap()
        {
            Assert.AreEqual(1, ts.vwap, 0.001, "Vwap not calculated correctly");
        }
        /* TODO: MOCK THESE
        [TestMethod]
        public async Task testDefaults()
        {
            TradeData tradeData = new TradeData();       // test default constructor
            tradeData.client = Mock
            await tradeData.QueryTradeData();            // test query and calc
            
            tradeData.calculateStats();
            Assert.AreNotEqual(0, tradeData.volume, 0);  // assert that there is actually some data we were able to fetch and calculate
        }

        

        

        [TestMethod]
        public async Task testBitstamp()
        {
            TradeData tradeData = new TradeData(15, Exchange.Bitstamp, "btcusd");       // test default constructor
            await tradeData.QueryTradeData();            // test query and calc
            tradeData.calculateStats();
            Assert.AreNotEqual(0, tradeData.volume, 0);  // assert that there is actually some data we were able to fetch and calculate
        }

        [TestMethod]
        public async Task testKraken()
        {
            TradeData tradeData = new TradeData(20, Exchange.Kraken, "btcusd");       // test default constructor
            await tradeData.QueryTradeData();            // test query and calc
            tradeData.calculateStats();
            Assert.AreNotEqual(0, tradeData.volume, 0);  // assert that there is actually some data we were able to fetch and calculate
        }
        */
    }
}
