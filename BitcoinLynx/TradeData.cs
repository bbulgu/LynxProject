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
        string url;              // url to get (depends on api)
        public HttpClient client;
        public List<Transaction> listOfTransactions = new List<Transaction>(); // List of transactions that took place from timespan to now

        public TradeStats tradeStats;

        int max_retries = 5;
        int retry_delay = 1000;
        int retries = 0;


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
            url = ApiRequestBuilder.getUrl(api, currencypair, timestamp);
            client = new HttpClient();
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
            while (retries < max_retries)
            {
                try
                {
                    // Perform the GET request
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string jsonString = await response.Content.ReadAsStringAsync();
                        ApiParser parser = ApiParserFactory.GetApiParser(api, timestamp);
                        listOfTransactions = parser.processJsonString(jsonString);
                        return;
                    }
                    // TODO: What else in the error handling?
                    else
                    {
                        Console.WriteLine($"Request for api {api} failed with status code: {response.StatusCode}");
                        retries++;
                        Console.WriteLine($"Retrying for the {retries} time after a {retry_delay}ms delay. Will stop after {max_retries} times.");
                        await Task.Delay(1000); 
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
        
    }

}
