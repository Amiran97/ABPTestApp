using System.Text.Json.Serialization;

namespace ABPTestApp.Models.DTOs
{
    public class CountDevicesOfValueResponse
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonPropertyName("countDevices")]
        public int CountDevices { get; set; }
    }
}
