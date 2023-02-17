using ABPTestApp.Models;
using ABPTestApp.Models.DTOs;
using MediatR;

namespace ABPTestApp.Domains.Experiment.Queries
{
    public class GetStatisticsQuery : IRequest<StatisticsResponse>
    {
    }
}
