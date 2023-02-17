using MediatR;

namespace ABPTestApp.Domains.Experiment.Queries
{
    public class GetPriceQuery : IRequest<string>
    {
        public string? DeviceToken { get; set; }
    }
}
