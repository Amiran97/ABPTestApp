using System.Text.Json.Serialization;

namespace ABPTestApp.Models.DTOs
{
    public class CountDevicesOfValuesByKeyResponse
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("countDevicesOfValues")]
        public ICollection<CountDevicesOfValueResponse>? CountDevicesOfValues { get; set; }
    }
}
