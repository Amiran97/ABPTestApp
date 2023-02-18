using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models;
using ABPTestApp.Services;
using MediatR;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetExperimentQueryHandler : IRequestHandler<GetExperimentQuery, string>
    {
        private readonly static Random random = new Random(DateTime.Now.Millisecond);
        private readonly IExperimentRepository repository;

        private readonly static ICollection<ExperimentValue> buttonColors = new List<ExperimentValue>() {
            new ExperimentValue { Value = "#FF0000", Percent = 33.3f },
            new ExperimentValue { Value = "#00FF00", Percent = 33.3f },
            new ExperimentValue { Value = "#0000FF", Percent = 33.3f }
        };
        private readonly static ICollection<ExperimentValue> prices = new List<ExperimentValue>() {
            new ExperimentValue { Value = "10", Percent = 75 },
            new ExperimentValue { Value = "20", Percent = 10 },
            new ExperimentValue { Value = "50", Percent = 5 },
            new ExperimentValue { Value = "5", Percent = 10 }
        };

        public GetExperimentQueryHandler(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        private string GetValue(string key)
        {
            ICollection<ExperimentValue> values = null;
            switch(key)
            {
                case "button-color":
                    values = buttonColors;
                    break;
                case "price":
                    values = prices;
                    break;
                default:
                    throw new ArgumentException("Invalid key!");
            }
            var maxPercent = values.Sum(p => p.Percent);
            var random = new Random();
            var randNum = random.Next((int)maxPercent);

            float stepSum = 0;
            foreach (var value in values)
            {
                if (stepSum <= randNum && randNum < (stepSum + value.Percent))
                {
                    return value.Value;
                }
                stepSum += value.Percent;
            }

            throw new Exception();
        }


        public async Task<string> Handle(GetExperimentQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.DeviceToken) && !string.IsNullOrWhiteSpace(request.Key))
            {
                var result = await repository.GetExperimentValueByDeviceTokenAndKeyAsync(request.DeviceToken, request.Key);
                if (result == null)
                {
                    result = GetValue(request.Key);
                    await repository.CreateAsync(request.DeviceToken, request.Key, result);
                }
                return result;
            }
            else
            {
                throw new NullReferenceException("DeviceToken and Key must be not null");
            }
        }
    }
}