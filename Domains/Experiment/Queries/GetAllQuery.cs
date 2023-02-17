using ABPTestApp.Models;
using MediatR;

namespace ABPTestApp.Domains.Experiment.Queries
{
    public class GetAllQuery : IRequest<ICollection<ABPTestApp.Models.Experiment>>
    {
    }
}
