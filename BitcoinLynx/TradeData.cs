using Newtonsoft.Json;

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
        Exchange api;           // which api to use
        string currencypair;    // which currency pair to query for


        string timestamp;        // a unix time stamp: time to query from (to now)
        string url;              // url to get (depends on api)
        HttpClient client;
        public List<Transaction> listOfTransactions = new List<Transaction>(); // List of transactions that took place from timespan to now
        public double volume;
        public double vwap;

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

        private string calculateTimeStamp(int mins_ago)
        {
            return (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - mins_ago * 60).ToString();
        }

        private void initAttributes()
        {
            timestamp = calculateTimeStamp(this.mins_ago);
            url = getUrl();
            client = new HttpClient();
            listOfTransactions = new List<Transaction>();
            volume = 0;
            vwap = 0;
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

        public async Task queryAndCalculateAsync()
        {
            await QueryTradeData();
            calculateStats();
            printStats();
        }


        // method to query the chosen api with the chosen currency pair
        public async Task QueryTradeData()
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
                        List<GeminiTransaction> geminiTransactions = JsonConvert.DeserializeObject<List<GeminiTransaction>>(jsonString) ?? new List<GeminiTransaction>();
                        geminiTransactions.ForEach(x =>
                        {
                            listOfTransactions.Add(new Transaction(price: x.price, amount: x.amount));
                        });
                    }

                    // TODO: Add some error handling in the type conversions here
                    else if (api.Equals(Exchange.Bitstamp))
                    {
                        List<BitstampTransaction> geminiTransactions = JsonConvert.DeserializeObject<List<BitstampTransaction>>(jsonString) ?? new List<BitstampTransaction>();
                        int t = 0;
                        Int32.TryParse(timestamp, out t);
                        geminiTransactions.ForEach(x =>
                        {
                            // we need to check if the date is within our bounds
                            // bitstamp api does not provide filtering given a timestamp so we must do it ourselves here
                            if (x.date > t)
                                listOfTransactions.Add(new Transaction(price: x.price, amount: x.amount));
                        });
                    }

                    else if (api.Equals(Exchange.Kraken))
                    {
                        KrakenRootObject root = JsonConvert.DeserializeObject<KrakenRootObject>(jsonString) ?? new KrakenRootObject();
                        if (root != null)
                        {
                            root.Result.XXBTZUSD.ForEach(x =>
                            {
                                Console.WriteLine(x[0]);
                                Console.WriteLine(x[1]);
                                double tryPrice = 0;
                                double tryAmount = 0;
                                Double.TryParse(x[0].ToString(), out tryPrice);
                                Double.TryParse(x[1].ToString(), out tryAmount);
                                Console.WriteLine(tryPrice);
                                if (tryPrice !=0 && tryAmount != 0)
                                {
                                    listOfTransactions.Add(new Transaction(price: tryPrice, amount: tryAmount));
                                }                                
                            });
                        }
                        
                    }
                }
                // TODO: What else in the error handling?
                else
                {
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    
        void calculateVolume()
        {
            listOfTransactions.ForEach(t => volume += t.amount);
        }

        void calculateVwap()
        {
            double total_quantity = 0;
            foreach (Transaction transaction in listOfTransactions)
            {
                vwap += transaction.price * transaction.amount;
                total_quantity += transaction.amount;
            }
            vwap = vwap / total_quantity;
        }
        public void calculateStats()
        {
            calculateVolume();
            calculateVwap();
        }

        void printStats()
        {
            Console.WriteLine($"For the transactions of the currency pair {currencypair} that took place in the last {mins_ago} minutes in the exchange {api}:");
            Console.WriteLine($"Volume: {volume}, vwap: {vwap}");
        }
    }






}
