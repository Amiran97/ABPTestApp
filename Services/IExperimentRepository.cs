using ABPTestApp.Models;

namespace ABPTestApp.Services
{
    public interface IExperimentRepository
    {
        Task CreateAsync(string DeviceToken, string Key, string Value);
        Task CreateAsync(Experiment experiment);
        Task<ICollection<Experiment>> GetDataAsync();
    }
}
