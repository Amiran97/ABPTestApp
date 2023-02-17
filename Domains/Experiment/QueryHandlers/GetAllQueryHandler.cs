using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Services;
using MediatR;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ICollection<ABPTestApp.Models.Experiment>>
    {
        private readonly IExperimentRepository repository;

        public GetAllQueryHandler(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ICollection<Models.Experiment>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetDataAsync();
        }
    }
}
