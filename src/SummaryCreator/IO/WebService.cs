using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.IO
{
    public class WebService
    {
        private readonly HttpClient httpClient;

        public WebService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<string> DownloadAsync(string resource, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
        {
            var requestUri = $"{resource}?from={from:s}&to={to:s}";

            return httpClient.GetStringAsync(requestUri, cancellationToken);
        }
    }
}
