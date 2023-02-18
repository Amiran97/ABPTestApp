using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models.DTOs;
using ABPTestApp.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsResponse>
    {
        private readonly IExperimentRepository repository;
        private readonly IDistributedCache cache;

        public GetStatisticsQueryHandler(IExperimentRepository repository, IDistributedCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public async Task<StatisticsResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var jsonData = await cache.GetStringAsync("statistics");
            if(string.IsNullOrWhiteSpace(jsonData))
            {
                var result = await repository.GetStatisticsAsync();
                var jsonToSave = JsonSerializer.Serialize(result);
                await cache.SetStringAsync("statistics", jsonToSave);
                return result;
            } else
            {
                var result = JsonSerializer.Deserialize<StatisticsResponse>(jsonData);
                if (result != null)
                    return result;
                else
                    throw new Exception("Redis containt empty statistics data");
            }
        }
    }
}
