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

            float stepSum = 0;
            foreach (var price in prices)
            {
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
            if (request.DeviceToken != null)
            {
                var result = await repository.GetExperimentValueByDeviceTokenAndKeyAsync(request.DeviceToken, "price");
                if (result == null)
                {
                    result = await GetPrice();
                    await repository.CreateAsync(request.DeviceToken, "price", result);
                }
                return result;
            }
            else
            {
                throw new NullReferenceException("DeviceToken must be not null");
            }
        }
    }
}
