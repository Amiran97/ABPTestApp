using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ABPTestApp.Models.DTOs
{
    public class ExperimentRequest
    {
        [BindProperty(Name = "device-token")]
        [SwaggerParameter("Enter device token for experiment!")]
        public string? DeviceToken { get; set; }
    }
}