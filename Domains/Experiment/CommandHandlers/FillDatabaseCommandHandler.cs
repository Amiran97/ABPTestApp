using ABPTestApp.Domains.Experiment.Commands;
using ABPTestApp.Services;
using ABPTestApp.Utils;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ABPTestApp.Domains.Experiment.CommandHandlers
{
    public class FillDatabaseCommandHandler : IRequestHandler<FillDatabaseCommand, Unit>
    {
        private readonly IExperimentRepository repository;
        private readonly IDistributedCache cache;

        public FillDatabaseCommandHandler(IExperimentRepository repository, IDistributedCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public async Task<Unit> Handle(FillDatabaseCommand request, CancellationToken cancellationToken)
        {
            for(int i = 1; i <= 601; i++)
            {
                try
                {
                    await repository.CreateAsync($"test{i}", "button-color", ExperimentUtil.GetValue("button-color"));
                    await repository.CreateAsync($"test{i}", "price", ExperimentUtil.GetValue("price"));
                } catch(Exception ex)
                {
                    
                }
            }
            await cache.RemoveAsync("statistics");
            return Unit.Value;
        }
    }
}
