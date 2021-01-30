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

        public Task<SummaryCreatorConfig> LoadAsync(string filePath, CancellationToken cancellationToken = default)
        {
            using FileStream fileStream = File.Open(filePath, FileMode.Open);
            return configurationConverter.ConvertAsync(fileStream, cancellationToken);
        }
    }
}
