using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string date;
    }
}
