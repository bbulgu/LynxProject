
namespace BitcoinLynx
{
    internal interface ApiParser
    {
        List<Transaction> processJsonString(string json);
    }
}
