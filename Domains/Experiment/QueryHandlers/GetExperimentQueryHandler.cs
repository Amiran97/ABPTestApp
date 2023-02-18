using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models;
using ABPTestApp.Services;
using ABPTestApp.Utils;
using MediatR;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetExperimentQueryHandler : IRequestHandler<GetExperimentQuery, string>
    {
        private readonly IExperimentRepository repository;

        public GetExperimentQueryHandler(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Handle(GetExperimentQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.DeviceToken) && !string.IsNullOrWhiteSpace(request.Key))
            {
                var result = await repository.GetExperimentValueByDeviceTokenAndKeyAsync(request.DeviceToken, request.Key);
                if (result == null)
                {
                    result = ExperimentUtil.GetValue(request.Key);
                    await repository.CreateAsync(request.DeviceToken, request.Key, result);
                }
                return result;
            }
            else
            {
                throw new NullReferenceException("DeviceToken and Key must be not null");
            }
        }
    }
}