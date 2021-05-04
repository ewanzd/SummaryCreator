using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SummaryCreator.Input
{
    public class FileService : IFileService
    {
        private const string URI_SCHEMA_FILE = "file";
        private const string HTTP_QUERY_PARAM_FROM = "from";
        private const string HTTP_QUERY_PARAM_TO = "to";

        public async Task<string> LoadAsync(string resource, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(resource);
            string content;
            if (uriBuilder.Scheme.Equals(URI_SCHEMA_FILE))
            {
                content = await ReadFromFile(uriBuilder, cancellationToken);
            }
            else
            {
                content = await DownloadFromWeb(uriBuilder, from, to, cancellationToken);
            }

            return content;
        }

        private Task<string> ReadFromFile(UriBuilder uriBuilder, CancellationToken cancellationToken)
        {
            return File.ReadAllTextAsync(uriBuilder.Uri.LocalPath, cancellationToken);
        }

        private Task<string> DownloadFromWeb(UriBuilder uriBuilder, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();

            uriBuilder.Port = -1;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[HTTP_QUERY_PARAM_FROM] = $"{from:s}";
            query[HTTP_QUERY_PARAM_TO] = $"{to:s}";
            uriBuilder.Query = query.ToString();

            var requestUri = uriBuilder.Uri;

            return httpClient.GetStringAsync(requestUri, cancellationToken);
        }

        public FileStream Open(string resource)
        {
            return File.Open(resource, FileMode.Open, FileAccess.ReadWrite);
        }
    }
}
