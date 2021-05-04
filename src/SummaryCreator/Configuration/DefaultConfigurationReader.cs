using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Configuration
{
    public class DefaultConfigurationReader : IConfigurationReader
    {
        private readonly IConfigurationParser configurationConverter;

        public DefaultConfigurationReader(IConfigurationParser configurationConverter)
        {
            this.configurationConverter = configurationConverter ?? throw new ArgumentNullException(nameof(configurationConverter));
        }

        public async Task<SummaryCreatorConfig> ReadAsync(FileInfo filePath, CancellationToken cancellationToken = default)
        {
            await using FileStream fileStream = filePath.OpenRead();
            return await configurationConverter.ParseAsync(fileStream, cancellationToken).ConfigureAwait(false);
        }
    }
}
