using SummaryCreator.Configuration;
using SummaryCreator.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SummaryCreator.Services
{
    public interface ITimeSeriesService
    {
        Task<IEnumerable<ITimeSeries>> ReadAsync(IEnumerable<MeteoConfig> config);

        Task<IEnumerable<ITimeSeries>> ReadAsync(IEnumerable<EnergyConfig> config);

        Task WriteAsync(IEnumerable<ITimeSeries> timeSeries, IEnumerable<SummaryConfig> excelConfigs);
    }
}
