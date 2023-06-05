using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLynx
{
    public class TradeStats
    {
        public static double calculateVolume(List<Transaction> transactions)
        {
            double volume = 0;
            transactions.ForEach(t => volume += t.amount);
            return volume;
        }

        public static double calculateVwap(List<Transaction> transactions)
        {
            double volume = calculateVolume(transactions);
            double vwap = 0;
            foreach (Transaction transaction in transactions)
            {
                vwap += transaction.price * transaction.amount;
            }
            return vwap / volume;
        }
        public static void calculateStats(List<Transaction> transactions)
        {
            calculateVolume(transactions);
            calculateVwap(transactions);
        }
    }
}
