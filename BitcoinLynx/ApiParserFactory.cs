using BitcoinLynx.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx
{
    public class ApiParserFactory
    {
        public static ApiParser GetApiParser(Exchange api, string? timestamp)
        {
            ApiParser apiParser = new GeminiParser();    // default is gemini
            if (api.Equals(Exchange.Bitstamp))
                apiParser = new BitstampParser(timestamp);
            else if (api.Equals(Exchange.Kraken))
                apiParser = new KrakenParser();
            return apiParser;
        }

    }
}
