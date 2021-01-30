using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Configuration
{
    public class DefaultConfigurationReader : IConfigurationReader
    {
        private readonly IConfigurationConverter configurationConverter;

        public DefaultConfigurationReader(IConfigurationConverter configurationConverter)
        {
            this.configurationConverter = configurationConverter ?? throw new ArgumentNullException(nameof(configurationConverter));
        }

        public async Task<SummaryCreatorConfig> LoadAsync(FileInfo filePath, CancellationToken cancellationToken = default)
        {
            await using FileStream fileStream = filePath.OpenRead();
            return await configurationConverter.ConvertAsync(fileStream, cancellationToken).ConfigureAwait(false);
        }
    }
}
