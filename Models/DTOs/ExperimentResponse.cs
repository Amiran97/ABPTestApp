using System.Text.Json.Serialization;

namespace ABPTestApp.Models.DTOs
{
    public class ExperimentResponse
    {
        [JsonPropertyName("deviceToken")]
        public string? DeviceToken { get; set; }
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
