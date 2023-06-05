namespace BitcoinLynx
{
    public class Utils
    {
        public static string calculateTimeStamp(int mins_ago)
        {
            return (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - mins_ago * 60).ToString();
        }
    }
}
