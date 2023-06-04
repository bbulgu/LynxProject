﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;

namespace BitcoinLynx
{
    class BitcoinLynx
    {
        static Timer timer;

        static void Main(string[] args)
        {
            // Create an instance of HttpClient

            string unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            TradeData tradeData2mins = new TradeData(2, Exchange.Gemini, "btcusd");


            // Create a timer that triggers the query every 2 minutes
            timer = new Timer(async x => {
                TradeData tradeData2mins = new TradeData(2, Exchange.Gemini, "btcusd");
                await tradeData2mins.queryAndCalculateAsync();
                TradeData tradeData10mins = new TradeData(10, Exchange.Gemini, "btcusd");
                await tradeData10mins.queryAndCalculateAsync();
                double vwap2mins = tradeData2mins.vwap;
                double vwap10mins = tradeData10mins.vwap;
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