using System.Text.Json.Serialization;

namespace ABPTestApp.Models.DTOs
{
    public class StatisticsResponse
    {
        [JsonPropertyName("experiments")]
        public ICollection<ExperimentResponse>? Experiments { get; set; }
        [JsonPropertyName("countDevices")]
        public int CountDevices { get; set; }
        [JsonPropertyName("countDevicesOfValuesByKeys")]
        public ICollection<CountDevicesOfValuesByKeyResponse>? CountDevicesOfValuesByKeys { get; set; }
    }
}
