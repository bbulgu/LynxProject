using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx
{
    public class KrakenParser : ApiParser
    {
        List<Transaction> listOfTransactions = new List<Transaction>();
        public List<Transaction> processJsonString(string jsonString)
        {
            KrakenRootObject root = JsonConvert.DeserializeObject<KrakenRootObject>(jsonString) ?? new KrakenRootObject();
            if (root != null)
            {
                root.Result.XXBTZUSD.ForEach(x =>
                {
                    double tryPrice = 0;
                    double tryAmount = 0;
                    Double.TryParse(x[0].ToString(), out tryPrice);
                    Double.TryParse(x[1].ToString(), out tryAmount);
                    if (tryPrice != 0 && tryAmount != 0)
                    {
                        listOfTransactions.Add(new Transaction(price: tryPrice, amount: tryAmount));
                    }
                });
            }
            return listOfTransactions;
        }
    }
}
