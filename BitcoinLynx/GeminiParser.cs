using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx
{
    public class GeminiParser : ApiParser
    {
        List<Transaction> listOfTransactions = new List<Transaction>();
        public List<Transaction> processJsonString(string jsonString)
        {
            List<GeminiTransaction> geminiTransactions = JsonConvert.DeserializeObject<List<GeminiTransaction>>(jsonString) ?? new List<GeminiTransaction>();
            geminiTransactions.ForEach(x =>
            {
                listOfTransactions.Add(new Transaction(price: x.price, amount: x.amount));
            });

            return listOfTransactions;
        }
    }
}
