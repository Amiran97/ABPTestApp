using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models.DTOs;
using ABPTestApp.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ABPTestApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISender mediator;
        public StatisticsResponse? Statistics { get; set; }

        public IndexModel(ISender mediator)
        {
            this.mediator = mediator;
        }

        public async Task OnGetAsync()
        {
            Statistics = await mediator.Send(new GetStatisticsQuery());
        }
    }
}
