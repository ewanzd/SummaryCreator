using System;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Services
{
    public interface IFileService
    {
        Task<string> LoadAsync(string resource, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);
    }
}
