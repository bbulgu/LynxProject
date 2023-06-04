using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx
{
    public class BitstampParser : ApiParser
    {
        String timestamp;
        List<Transaction> listOfTransactions = new List<Transaction>();

        public BitstampParser(String timestamp)
        {
            this.timestamp = timestamp;
        }
        public List<Transaction> processJsonString(string jsonString)
        {
            List<Transaction> listOfTransactions = new List<Transaction> ();
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
            return listOfTransactions;
        }
    }
}
