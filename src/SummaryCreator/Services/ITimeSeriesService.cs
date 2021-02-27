using SummaryCreator.Configuration;
using SummaryCreator.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SummaryCreator.Services
{
    public interface ITimeSeriesService
    {
        Task<IEnumerable<ITimeSeries>> ReadAsync(TimeSeriesConfig config);

        Task WriteAsync(IEnumerable<ITimeSeries> timeSeries, IEnumerable<ExcelConfig> excelConfigs);
    }
}
