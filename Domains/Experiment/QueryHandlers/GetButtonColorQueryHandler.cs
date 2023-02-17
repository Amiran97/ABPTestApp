using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models;
using ABPTestApp.Services;
using MediatR;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetButtonColorQueryHandler : IRequestHandler<GetButtonColorQuery, string>
    {
        private readonly static Random random = new Random(DateTime.Now.Millisecond);
        private readonly IExperimentRepository repository;

        private readonly static ICollection<ButtonColor> buttonColors = new List<ButtonColor>() {
            new ButtonColor { Color = "#FF0000", Percent = 33.3f },
            new ButtonColor { Color = "#00FF00", Percent = 33.3f },
            new ButtonColor { Color = "#0000FF", Percent = 33.3f }
        };

        public GetButtonColorQueryHandler(IExperimentRepository repository)
        {
            this.repository = repository;
        }

        private Task<string> GetColor()
        {
            var maxPercent = buttonColors.Sum(bc => bc.Percent);
            var random = new Random();
            var randNum = random.Next((int)maxPercent);
            Console.WriteLine(maxPercent);
            Console.WriteLine(randNum);

            float stepSum = 0;
            foreach (var buttonColor in buttonColors)
            {
                Console.WriteLine("Min: " + stepSum + ", Max: " + (stepSum + buttonColor.Percent));
                if (stepSum <= randNum && randNum < (stepSum + buttonColor.Percent))
                {
                    return Task.FromResult(buttonColor.Color);
                }
                stepSum += buttonColor.Percent;
            }

            throw new Exception();
        }

        public async Task<string> Handle(GetButtonColorQuery request, CancellationToken cancellationToken)
        {
            if(request.DeviceToken != null)
            {
                var result = await repository.GetExperimentValueByDeviceTokenAndKey(request.DeviceToken, "button-color");
                if (result == null)
                {
                    result = await GetColor();
                    await repository.CreateAsync(request.DeviceToken, "button-color", result);
                }
                return result;
            } else
            {
                throw new NullReferenceException("DeviceToken must be not null");
            }
        }
    }
}
