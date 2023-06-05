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

                       // only btcusd is supported in my kraken parser for now
                       if (api == Exchange.Kraken)
                       {
                           currencypair = "btcusd";
                       }

                   });

            Console.WriteLine($"Initialized with api {api}, currencypair {currencypair}");

            string unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            // Create a timer that triggers the query every 2 minutes
            // TODO: This could be configurable as well (the frequency of the queries)
            timer = new Timer(async x => {
                TradeData tradeData2mins = new TradeData(2, api, currencypair);
                double vwap2mins = await tradeData2mins.queryAndCalculateVwapAsync();
                TradeData tradeData10mins = new TradeData(10, api, currencypair);
                double vwap10mins = await tradeData10mins.queryAndCalculateVwapAsync();

                Console.WriteLine($"Vwap for the last 2 mins: {vwap2mins}, vwap for the last 10 mins: {vwap10mins}");


                // add a sanity check
                if (vwap2mins == 0 || vwap10mins == 0)
                {
                    Console.WriteLine("Vwap calculation returned zero, you might want to try a different api and/or currency pair");
                }

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