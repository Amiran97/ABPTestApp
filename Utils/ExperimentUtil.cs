using ABPTestApp.Models;

namespace ABPTestApp.Utils
{
    public static class ExperimentUtil
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);
        public static ICollection<ExperimentValue> ButtonColors { get; } = new List<ExperimentValue>() {
            new ExperimentValue { Value = "#FF0000", Percent = 33.3f },
            new ExperimentValue { Value = "#00FF00", Percent = 33.3f },
            new ExperimentValue { Value = "#0000FF", Percent = 33.3f }
        };
        public static ICollection<ExperimentValue> Prices { get; } = new List<ExperimentValue>() {
            new ExperimentValue { Value = "10", Percent = 75 },
            new ExperimentValue { Value = "20", Percent = 10 },
            new ExperimentValue { Value = "50", Percent = 5 },
            new ExperimentValue { Value = "5", Percent = 10 }
        };

        public static string GetValue(string key)
        {
            ICollection<ExperimentValue> values = null;
            switch (key)
            {
                case "button-color":
                    values = ButtonColors;
                    break;
                case "price":
                    values = Prices;
                    break;
                default:
                    throw new ArgumentException("Invalid key!");
            }
            var maxPercent = values.Sum(p => p.Percent);
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
    }
}
