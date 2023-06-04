using BitcoinLynx;

namespace BitcoinLynxTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void testBitstampParser()
        {
            string samplejson = "[  {     \"amount\":\"0.00246247\",     \"date\":\"1685916109\",     \"price\":\"27213\",     \"tid\":\"287313268\",     \"type\":\"0\"  },  {     \"amount\":\"0.00246284\",     \"date\":\"1685916119\",     \"price\":\"27210\",     \"tid\":\"287313267\",     \"type\":\"0\"  },  {     \"amount\":\"0.00100000\",     \"date\":\"1685916071\",     \"price\":\"27201\",     \"tid\":\"287313225\",     \"type\":\"0\"  },  {     \"amount\":\"0.00342000\",     \"date\":\"1685916067\",     \"price\":\"27201\",     \"tid\":\"287313222\",     \"type\":\"0\"  }\r\n]";
            BitstampParser bitstampParser = new BitstampParser("1685916110");
            List<Transaction> listOfTransactions = bitstampParser.processJsonString(samplejson);

            Assert.IsNotNull(listOfTransactions); 
            Assert.AreEqual(1, listOfTransactions.Count);
            Assert.AreEqual(0.00246284, listOfTransactions.First().amount);
            Assert.AreEqual(27210, listOfTransactions.First().price);
        }

        [TestMethod]
        public void testGeminiParser()
        {
            string samplejson = "[   {       \"timestamp\": 1685804982,       \"timestampms\": 1685804982699,       \"tid\": 178554034314,      \"price\": \"27207.33\",      \"amount\": \"0.01080669\",      \"exchange\": \"gemini\",      \"type\": \"buy\"  },  {      \"timestamp\": 1685804979,      \"timestampms\": 1685804979914,      \"tid\": 178554024964,      \"price\": \"27207.32\",      \"amount\": \"0.00002307\",      \"exchange\": \"gemini\",      \"type\": \"sell\"  },  {      \"timestamp\": 1685804940,      \"timestampms\": 1685804940897,      \"tid\": 178553962546,      \"price\": \"27206.35\",      \"amount\": \"0.00067697\",      \"exchange\": \"gemini\",      \"type\": \"buy\"  },  {      \"timestamp\": 1685804937,      \"timestampms\": 1685804937586,      \"tid\": 178553955696,      \"price\": \"27204.70\",      \"amount\": \"0.00275795\",      \"exchange\": \"gemini\",      \"type\": \"sell\"  }]";
            GeminiParser geminiParser = new GeminiParser();
            List<Transaction> listOfTransactions = geminiParser.processJsonString(samplejson);

            Assert.IsNotNull(listOfTransactions);
            Assert.AreEqual(4, listOfTransactions.Count);
            Assert.AreEqual(0.01080669, listOfTransactions.First().amount);
            Assert.AreEqual(27207.33, listOfTransactions.First().price);
        }

        [TestMethod]
        public void testKrakenParser()
        {
            string samplejson = "{\"error\":[],\"result\":{\"XXBTZUSD\":[[\"27066.80000\",\"0.00919129\",1685713298.177992,\"b\",\"l\",\"\",59796572],[\"27066.70000\",\"0.00920437\",1685713314.4194527,\"s\",\"l\",\"\",59796573],[\"27064.80000\",\"0.00106350\",1685713322.2329805,\"b\",\"m\",\"\",59796574]]}}";
            KrakenParser krakenParser = new KrakenParser();
            List<Transaction> listOfTransactions = krakenParser.processJsonString(samplejson);

            Assert.IsNotNull(listOfTransactions);
            Assert.AreEqual(3, listOfTransactions.Count);
            Assert.AreEqual(0.00919129, listOfTransactions.First().amount);
            Assert.AreEqual(27066.80000, listOfTransactions.First().price);
        }
    }
}
