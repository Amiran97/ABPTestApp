using MediatR;

namespace ABPTestApp.Domains.Experiment.Queries
{
    public class GetButtonColorQuery : IRequest<string>
    {
        public string? DeviceToken { get; set; }
    }
}
