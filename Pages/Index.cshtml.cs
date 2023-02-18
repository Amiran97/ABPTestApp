using ABPTestApp.Models.DTOs;
using ABPTestApp.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ABPTestApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IExperimentRepository repository;
        public StatisticsResponse? Statistics { get; set; }

        public IndexModel(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        public async Task OnGetAsync()
        {
            Statistics = await repository.GetStatisticsAsync();
            
        }

        public void OnClick()
        {
            Console.WriteLine("Click event");
        }
    }
}
