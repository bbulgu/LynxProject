using System.Text.Json.Serialization;

namespace BitcoinLynx
{
    public class Transaction
    {
        // There are other fields but these two are all we need and it looks like
        // All three apis share the same names
        public double price { get; set; }
        public double amount { get; set; }

        public Transaction() { }

        public Transaction(double price, double amount)
        {
            this.price = price;
            this.amount = amount;
        }
    }

    public class GeminiTransaction : Transaction { 
        
    }

    public class BitstampTransaction : Transaction
    {
        public int date;
    }

      public class KrakenResult
    {
        // TODO: This name won't work for different currency pairs!
        [JsonPropertyName("XXBTZUSD")]
        public List<List<object>> XXBTZUSD { get; set; }
        public string Last { get; set; }
    }

    public class KrakenRootObject
    {
        public List<string> Error { get; set; }
        public KrakenResult Result { get; set; }
    }
}
