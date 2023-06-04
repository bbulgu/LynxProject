using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx
{
    public class Transaction
    {
        // There are other fields but these two are all we need and it looks like
        // All three apis share the same names
        public double price { get; set; }
        public double amount { get; set; }

        public Transaction(double price, double amount)
        {
            this.price = price;
            this.amount = amount;
        }

        public Transaction(double v1, int v2)
        {
        }
    }
    public enum Exchange
    {
        Kraken,
        Gemini,
        Bitstamp
    }
    public class TradeData
    {
        int mins_ago;           // how many mins far back we wanna query
        string timestamp;        // a unix time stamp: time to query from
        Exchange api;           // which api to use
        string currencypair;    // which currency pair to query for
        HttpClient client;
        public List<Transaction> listOfTransactions = new List<Transaction>(); // List of transactions that took place from timespan to now
        public double volume;
        public double vwap;



        public TradeData(int mins_ago, Exchange api, string currencypair)
        {
            this.mins_ago = mins_ago;
            this.api = api;
            this.currencypair = currencypair;
            this.timestamp = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - mins_ago * 60).ToString();
            this.client = new HttpClient();
            listOfTransactions = new List<Transaction>();
            volume = 0;
            vwap = 0;
        }

        public TradeData()
        {
        }

        public async Task queryAndCalculateAsync()
        {
            await QueryTradeData();
            calculateStats();
            printStats();
        }


        // method to query the chosen api with the chosen currency pair
        async Task QueryTradeData()
        {
            try
            {
                if (api.Equals(Exchange.Gemini))
                { 
                // Perform the GET request
                    HttpResponseMessage response = await client.GetAsync($"https://api.gemini.com/v1/trades/{currencypair}?timestamp={timestamp}");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string jsonString = await response.Content.ReadAsStringAsync();

                        listOfTransactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonString) ?? new List<Transaction>();
                    }
                    // TODO: What else in the error handling?
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
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
