using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models.DTOs;
using ABPTestApp.Services;
using ABPTestApp.Utils;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetExperimentQueryHandler : IRequestHandler<GetExperimentQuery, string>
    {
        private readonly IExperimentRepository repository;
        private readonly IDistributedCache cache;

        public GetExperimentQueryHandler(IExperimentRepository repository, IDistributedCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public async Task<string> Handle(GetExperimentQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.DeviceToken) && !string.IsNullOrWhiteSpace(request.Key))
            {
                var result = await cache.GetStringAsync($"deviceToken:{request.DeviceToken}&&key:{request.Key}");
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = await repository.GetExperimentValueByDeviceTokenAndKeyAsync(request.DeviceToken, request.Key);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        result = ExperimentUtil.GetValue(request.Key);
                        await repository.CreateAsync(request.DeviceToken, request.Key, result);
                        await cache.RemoveAsync("statistics");
                    }
                    await cache.SetStringAsync($"deviceToken:{request.DeviceToken}&&key:{request.Key}", result);
                    return result;
                } else
                {
                    return result;
                }
            }
            else
            {
                throw new NullReferenceException("DeviceToken and Key must be not null");
            }
        }
    }
}