using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetPriceQueryHandler : IRequestHandler<GetPriceQuery, int>
    {
        private readonly static Random random = new Random(DateTime.Now.Millisecond);

        private readonly static ICollection<Price> prices = new List<Price>() {
            new Price { Value = 10, Percent = 75 },
            new Price { Value = 20, Percent = 10 },
            new Price { Value = 50, Percent = 5 },
            new Price { Value = 5, Percent = 10 }
        };

        private Task<int> getPrice()
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


        public async Task<int> Handle(GetPriceQuery request, CancellationToken cancellationToken)
        {
            return await getPrice();
        }
    }
}
