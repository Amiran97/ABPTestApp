using ABPTestApp.Models;

namespace ABPTestApp.Options
{
    public static class ExperimentValuesOptions
    {
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
    }
}
