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
        HttpClient client;
        public List<Transaction> listOfTransactions = new List<Transaction>(); // List of transactions that took place from timespan to now

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
            url = getUrl();
            client = new HttpClient();
            listOfTransactions = new List<Transaction>();
        }

        private string getUrl()
        {
            if (api.Equals(Exchange.Bitstamp))
            {
                return $"https://www.bitstamp.net/api/v2/transactions/{currencypair}/?time=hour";
            }
            else if (api.Equals(Exchange.Kraken))
            {
                return $"https://api.kraken.com/0/public/Trades?pair={currencypair}&since={timestamp}";
            }
            else  // default to gemini
            {
                return $"https://api.gemini.com/v1/trades/{currencypair}?timestamp={timestamp}";
            }

        }

        public async Task<double> queryAndCalculateVwapAsync()
        {
            await QueryTradeData();
            return TradeStats.calculateVwap(listOfTransactions);
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
                        if (api.Equals(Exchange.Gemini))
                        {
                            GeminiParser gemini = new GeminiParser();
                            listOfTransactions = gemini.processJsonString(jsonString);
                        }

                        // TODO: Add some error handling in the type conversions here
                        else if (api.Equals(Exchange.Bitstamp))
                        {
                            BitstampParser bitstamp = new BitstampParser(timestamp);
                            listOfTransactions = bitstamp.processJsonString(jsonString);
                        }

                        else if (api.Equals(Exchange.Kraken))
                        {
                            KrakenParser kraken = new KrakenParser();
                            listOfTransactions = kraken.processJsonString(jsonString);
                        }
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
        /*
        void printStats()
        {
            Console.WriteLine($"For the transactions of the currency pair {currencypair} that took place in the last {mins_ago} minutes in the exchange {api}:");
            Console.WriteLine($"Volume: {volume}, vwap: {vwap}");
        }
        */
    }






}
