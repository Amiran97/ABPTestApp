using MediatR;

namespace ABPTestApp.Domains.Experiment.Queries
{
    public class GetPriceQuery : IRequest<int>
    {
        public string? DeviceColor { get; set; }
    }
}
