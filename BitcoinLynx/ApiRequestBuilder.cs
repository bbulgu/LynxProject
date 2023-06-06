using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx
{
    public class ApiRequestBuilder
    { 
        public static string getUrl(Exchange api, string currencypair, string timestamp)
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
    }
}
