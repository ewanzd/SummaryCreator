using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Configuration
{
    /// <summary>
    /// Read and return <see cref="SummaryCreatorConfig"/> from a file.
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Read and return <see cref="SummaryCreatorConfig"/> from a file.
        /// </summary>
        /// <param name="file">Path to configuration file.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
        /// <returns>Filled configuration object.</returns>
        Task<SummaryCreatorConfig> LoadAsync(FileInfo file, CancellationToken cancellationToken = default);
    }
}
