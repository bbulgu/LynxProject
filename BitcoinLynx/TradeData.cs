using BitcoinLynx.ApiParsers;
using BitcoinLynx.Parser;
using static BitcoinLynx.Utils;
namespace BitcoinLynx
{
    public enum Exchange
    {
        Kraken,
        Gemini,
        Bitstamp
    }
    public class TradeData
    {
        int mins_ago;           // how many mins far back we wanna query
        protected Exchange api;           // which api to use
        string currencypair;    // which currency pair to query for

        string timestamp;        // a unix time stamp: time to query from (to now)

        public QueryEngine engine;
        public List<Transaction> listOfTransactions = new List<Transaction>(); // List of transactions that took place from timespan to now

        public TradeStats tradeStats;


        public TradeData() // init with defaults
        {
            mins_ago = 10;
            api = Exchange.Gemini;
            currencypair = "btcusd";
            initAttributes();
        }

        public TradeData(int mins_ago, Exchange api, string currencypair)
        {
            this.mins_ago = mins_ago;
            this.api = api;
            this.currencypair = currencypair;
            initAttributes();
        }


        private void initAttributes()
        {
            timestamp = calculateTimeStamp(this.mins_ago);
            listOfTransactions = new List<Transaction>();
        }

        public async Task queryAndCalculateAsync()
        {
            await QueryTradeData();
            tradeStats = new TradeStats(listOfTransactions);
            tradeStats.calculateStats();
        }


        // method to query the chosen api with the chosen currency pair
        public async Task QueryTradeData()
        {
            engine = new QueryEngine(ApiRequestBuilder.getUrl(api, currencypair, timestamp));
            string jsonString = await engine.queryApi();
            
            if (jsonString == null)
            {
                Console.WriteLine($"Request for api {api} failed.");
            }
            else
            {
                ApiParser parser = ApiParserFactory.GetApiParser(api, timestamp);
                listOfTransactions = parser.processJsonString(jsonString);
            }                 
        }
        
    }

}
