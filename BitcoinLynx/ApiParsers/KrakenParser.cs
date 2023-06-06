using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx.Parser
{
    public class KrakenParser : ApiParser
    {
        List<Transaction> listOfTransactions = new List<Transaction>();
        public List<Transaction> processJsonString(string jsonString)
        {
            KrakenRootObject root;
            try
            {
                root = JsonConvert.DeserializeObject<KrakenRootObject>(jsonString);
                if (root != null && root.Result != null)
                {
                    root.Result.XXBTZUSD.ForEach(x =>
                    {
                        double tryPrice = 0;
                        double tryAmount = 0;
                        double.TryParse(x[0].ToString(), out tryPrice);
                        double.TryParse(x[1].ToString(), out tryAmount);
                        if (tryPrice != 0 && tryAmount != 0)
                        {
                            listOfTransactions.Add(new Transaction(price: tryPrice, amount: tryAmount));
                        }
                    });
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("Error occurred during deserialization: " + ex.Message);
            }

            return listOfTransactions;
        }
    }
}
