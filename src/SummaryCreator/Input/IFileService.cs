using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Input
{
    public interface IFileService
    {
        Task<string> LoadAsync(string resource, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken);

        FileStream Open(string resource);
    }
}
