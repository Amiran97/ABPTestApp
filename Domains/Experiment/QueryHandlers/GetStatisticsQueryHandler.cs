using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models.DTOs;
using ABPTestApp.Services;
using MediatR;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsResponse>
    {
        private readonly IExperimentRepository repository;

        public GetStatisticsQueryHandler(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<StatisticsResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetStatisticsAsync();
        }
    }
}
