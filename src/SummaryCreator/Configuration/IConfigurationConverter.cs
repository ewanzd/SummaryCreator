using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Configuration
{
    /// <summary>
    /// Convert a stream to <see cref="SummaryCreatorConfig"/>.
    /// </summary>
    public interface IConfigurationConverter
    {
        /// <summary>
        /// Convert stream to <see cref="SummaryCreatorConfig"/>.
        /// </summary>
        /// <param name="contentStream">Stream with configurations.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
        /// <returns>Converted configuration.</returns>
        Task<SummaryCreatorConfig> ConvertAsync(Stream contentStream, CancellationToken cancellationToken = default);
    }
}
