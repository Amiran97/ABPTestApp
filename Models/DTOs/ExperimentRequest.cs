using Microsoft.AspNetCore.Mvc;

namespace ABPTestApp.Models.DTOs
{
    public class ExperimentRequest
    {
        [BindProperty(Name = "device-token")]
        public string? DeviceToken { get; set; }
    }
}