using ABPTestApp.Models;
using ABPTestApp.Models.DTOs;

namespace ABPTestApp.Services
{
    public interface IExperimentRepository
    {
        Task CreateAsync(string DeviceToken, string Key, string Value);
        Task CreateAsync(Experiment experiment);
        Task<ICollection<Experiment>> GetDataAsync();
        Task<string> GetExperimentValueByDeviceTokenAndKey(string DeviceToken, string Key);
    }
}
