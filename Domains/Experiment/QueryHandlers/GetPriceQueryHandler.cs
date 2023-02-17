using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models;
using ABPTestApp.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetPriceQueryHandler : IRequestHandler<GetPriceQuery, string>
    {
        private readonly static Random random = new Random(DateTime.Now.Millisecond);
        private readonly IExperimentRepository repository;

        private readonly static ICollection<Price> prices = new List<Price>() {
            new Price { Value = "10", Percent = 75 },
            new Price { Value = "20", Percent = 10 },
            new Price { Value = "50", Percent = 5 },
            new Price { Value = "5", Percent = 10 }
        };

        public GetPriceQueryHandler(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        private Task<string> GetPrice()
        {
            var maxPercent = prices.Sum(p => p.Percent);
            var random = new Random();
            var randNum = random.Next((int)maxPercent);
            Console.WriteLine(maxPercent);
            Console.WriteLine(randNum);

            float stepSum = 0;
            foreach (var price in prices)
            {
                Console.WriteLine("Min: " + stepSum + ", Max: " + (stepSum + price.Percent));
                if (stepSum <= randNum && randNum < (stepSum + price.Percent))
                {
                    return Task.FromResult(price.Value);
                }
                stepSum += price.Percent;
            }

            throw new Exception();
        }

        public async Task<string> Handle(GetPriceQuery request, CancellationToken cancellationToken)
        {
            var result = await GetPrice();
            await repository.CreateAsync(request.DeviceToken, "price", result);
            return result;
        }
    }
}
