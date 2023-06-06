namespace BitcoinLynx.Parser
{
    public interface ApiParser
    {
        List<Transaction> processJsonString(string json);
    }
}
