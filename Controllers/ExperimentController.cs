using ABPTestApp.Domains.Experiment.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestApp.Controllers
{
    [ApiController]
    [Route("api/experiment")]
    public class ExperimentController : ControllerBase
    {
        private readonly ISender mediator;

        public ExperimentController(ISender mediator) {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("button-color")]
        public async Task<IActionResult> GetButtonColorAsync([FromQuery(Name = "device-token")] string deviceToken)
        {
            string result = await mediator.Send(new GetButtonColorQuery() { DeviceColor = deviceToken });
            return Ok(result);
        }

        [HttpGet]
        [Route("price")]
        public async Task<IActionResult> GetPrice([FromQuery(Name = "device-token")] string deviceToken)
        {
            int result = await mediator.Send(new GetPriceQuery() { DeviceColor = deviceToken });
            return Ok(result);
        }
    }
}
