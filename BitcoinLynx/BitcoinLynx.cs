using CommandLine;

namespace BitcoinLynx
{
    class BitcoinLynx
    {
        static Timer timer;

        public class Options
        {
            [Option('a', "api", Required = false, HelpText = "Set which api to query.")]
            public string Api { get; set; }

            [Option('c', "currencypair", Required = false, HelpText = "Set which currency pair to query.")]
            public string CurrencyPair { get; set; }
        }

        static void Main(string[] args)
        {
            Exchange api = Exchange.Kraken;    // default
            string currencypair = "btcusd";
            

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       if (o.Api != null)
                       {
                           if (o.Api.Equals("gemini", StringComparison.OrdinalIgnoreCase))
                           {
                               api = Exchange.Gemini;
                           }
                           else if (o.Api.Equals("bitstamp", StringComparison.OrdinalIgnoreCase))
                           {
                               api = Exchange.Bitstamp;
                           }
                       }

                       if (o.CurrencyPair != null)
                       {
                           currencypair = o.CurrencyPair;
                       }

                   });
        

        string unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            // Create a timer that triggers the query every 2 minutes
            timer = new Timer(async x => {
                TradeData tradeData2mins = new TradeData(2, api, currencypair);
                double vwap2mins = await tradeData2mins.queryAndCalculateVwapAsync();
                TradeData tradeData10mins = new TradeData(10, api, currencypair);
                double vwap10mins = await tradeData10mins.queryAndCalculateVwapAsync();
                Console.WriteLine(api);
                if (vwap10mins > vwap2mins)
                {
                    Console.WriteLine("Price is going down.");
                }
                else if (vwap10mins < vwap2mins)
                {
                    Console.WriteLine("Price is going up.");
                }
                else
                {
                    Console.WriteLine("Price is the same.");
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            // Wait for user input to exit the program
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Clean up resources
            timer.Dispose();
        }

    }
}