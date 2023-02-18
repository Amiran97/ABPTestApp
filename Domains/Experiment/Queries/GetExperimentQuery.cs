using MediatR;

namespace ABPTestApp.Domains.Experiment.Queries
{
    public class GetExperimentQuery : IRequest<string>
    {
        public string? DeviceToken { get; set; }
        public string? Key { get; set; }
    }
}
