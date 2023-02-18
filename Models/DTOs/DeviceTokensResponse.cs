using System.Text.Json.Serialization;

namespace ABPTestApp.Models.DTOs
{
    public class DeviceTokensResponse
    {
        [JsonPropertyName("deviceToken")]
        public string? DeviceToken { get; set; }
    }
}
