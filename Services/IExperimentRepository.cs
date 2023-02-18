using ABPTestApp.Models;
using ABPTestApp.Models.DTOs;

namespace ABPTestApp.Services
{
    public interface IExperimentRepository
    {
        Task CreateAsync(string deviceToken, string key, string value);
        Task CreateAsync(Experiment experiment);
        Task<string> GetExperimentValueByDeviceTokenAndKeyAsync(string deviceToken, string key);
        Task<StatisticsResponse> GetStatisticsAsync();
    }
}
