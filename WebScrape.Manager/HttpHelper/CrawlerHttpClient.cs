using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebScrape.Manager.HttpHelper
{
    public class CrawlerHttpClient
    {
        private readonly System.Net.Http.HttpClient _client = null;
        public CrawlerKeyValuePairCollection Headers { get; } = new CrawlerKeyValuePairCollection();
        public CrawlerHttpClient()
        {
            var cookieContainer = new System.Net.CookieContainer();
            var httpClientHandler = new System.Net.Http.HttpClientHandler() { CookieContainer = cookieContainer };
            _client = new System.Net.Http.HttpClient(httpClientHandler);
        }
        public async Task<string> Get(string url)
        {         
            _client.DefaultRequestHeaders.Clear();
            foreach (var header in Headers.List)
            {
                if (header.Key.ToLowerInvariant() == "accept-encoding")
                    continue;
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            await Task.Delay(2000);
            var httpResponseString = await _client.GetStringAsync(url);
            return httpResponseString;
        }
    }
}
