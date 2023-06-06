using BitcoinLynx.ApiParsers;
using BitcoinLynx.Parser;


namespace BitcoinLynx
{
    
    public class QueryEngine
    {
        string url;              // url to get (depends on api)
        public HttpClient client;

        public QueryEngine(string url)
        {
            this.url = url;
            client = new HttpClient();
        }

        int max_retries = 5;
        int retry_delay = 1000;
        int retries = 0;

        // A simple engine to send get requests and return the json sent back from the server
        // Try for max_retries amount of times
        public async Task<string?> queryApi()
        {
            while (retries < max_retries)
            {
                try
                {
                    // Perform the GET request
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        return await response.Content.ReadAsStringAsync();
                    }
                    // TODO: What else in the error handling?
                    else
                    {
                        Console.WriteLine($"Request for url {url} failed with status code: {response.StatusCode}");
                        retries++;
                        Console.WriteLine($"Retrying for the {retries} time after a {retry_delay}ms delay. Will stop after {max_retries} times.");
                        await Task.Delay(1000);
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
            Console.WriteLine($"After {max_retries} retries, stopping.");
            return null;
        }
    
    }
}
