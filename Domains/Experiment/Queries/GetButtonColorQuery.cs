using MediatR;

namespace ABPTestApp.Domains.Experiment.Queries
{
    public class GetButtonColorQuery : IRequest<string>
    {
        public string? DeviceColor { get; set; }
    }
}
