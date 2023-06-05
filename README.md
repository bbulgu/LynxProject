﻿# LynxProject

A command line C# program to periodically check bitcoin prices. In particular, every 2 minutes it queries an api for every trade that happened within 2 and 10 minutes and uses the data returned to calculate vwap prices, and tell the user whether the price is going up or down.

The program has support for 3 different apis: gemini, bitstamp and kraken.
You can also specify the currency pair you want to compare (although only btcusd is supported if you choose to use kraken).

Example usage:
compile
run ./BitcoinLynx.exe -a gemini -c btcusd

cmdline options: 
-a gemini, -a bitstamp, -a kraken  (defaults to kraken)
-c btcusd, -c ... (any supported currency pair in gemini and bitstamp, but only btcusd for kraken)

