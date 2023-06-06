namespace BitcoinLynx
{
    public class TradeStats
    {
        public double vwap = 0;
        public double volume = 0;
        List<Transaction> transactions;

        public TradeStats(List<Transaction> transactions)
        {
            this.transactions = transactions;  
        }

        public void calculateVolume()
        {
            transactions.ForEach(t => volume += t.amount);           
        }

        public void calculateVwap()
        {
            if (transactions.Count == 0)
            {
                vwap = 0;
            }
                
            else { 
                foreach (Transaction transaction in transactions)
                {
                    vwap += transaction.price * transaction.amount;
                }
                vwap = vwap / volume;
            }
        }
        public void calculateStats()
        {
            calculateVolume();
            calculateVwap();
        }
    }
}
