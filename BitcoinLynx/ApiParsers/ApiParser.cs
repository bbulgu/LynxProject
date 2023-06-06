namespace BitcoinLynx.Parser
{
    internal interface ApiParser
    {
        List<Transaction> processJsonString(string json);
    }
}
