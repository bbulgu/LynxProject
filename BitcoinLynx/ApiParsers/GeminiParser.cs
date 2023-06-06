using Newtonsoft.Json;


namespace BitcoinLynx.Parser
{
    public class GeminiParser : ApiParser
    {
        List<Transaction> listOfTransactions = new List<Transaction>();
        public List<Transaction> processJsonString(string jsonString)
        {
            List<GeminiTransaction> geminiTransactions;
            try
            {
                geminiTransactions = JsonConvert.DeserializeObject<List<GeminiTransaction>>(jsonString) ?? new List<GeminiTransaction>();
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine("Error occurred during deserialization: " + ex.Message);
                return listOfTransactions;
            }
            geminiTransactions.ForEach(x =>
            {
                listOfTransactions.Add(new Transaction(price: x.price, amount: x.amount));
            });

            return listOfTransactions;
        }
    }
}
