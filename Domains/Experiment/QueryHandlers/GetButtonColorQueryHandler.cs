using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models;
using MediatR;

namespace ABPTestApp.Domains.Experiment.QueryHandlers
{
    public class GetButtonColorQueryHandler : IRequestHandler<GetButtonColorQuery, string>
    {
        private readonly static Random random = new Random(DateTime.Now.Millisecond);

        private readonly static ICollection<ButtonColor> buttonColors = new List<ButtonColor>() {
            new ButtonColor { Color = "#FF0000", Percent = 33.3f },
            new ButtonColor { Color = "#00FF00", Percent = 33.3f },
            new ButtonColor { Color = "#0000FF", Percent = 33.3f }
        };

        private Task<string> getColor()
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
            return await getColor();
        }
    }
}
