using ABPTestApp.Domains.Experiment.Commands;
using ABPTestApp.Models;
using ABPTestApp.Services;
using ABPTestApp.Utils;
using MediatR;

namespace ABPTestApp.Domains.Experiment.CommandHandlers
{
    public class FillDatabaseCommandHandler : IRequestHandler<FillDatabaseCommand, Unit>
    {
        private readonly IExperimentRepository repository;

        public FillDatabaseCommandHandler(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(FillDatabaseCommand request, CancellationToken cancellationToken)
        {
            for(int i = 1; i <= 601; i++)
            {
                await repository.CreateAsync($"test{i}", "button-color", ExperimentUtil.GetValue("button-color"));
                await repository.CreateAsync($"test{i}", "price", ExperimentUtil.GetValue("price"));
            }
            return Unit.Value;
        }
    }
}
