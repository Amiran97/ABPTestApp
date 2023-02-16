using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public async Task<IActionResult> GetButtonColorAsync([FromQuery] ExperimentRequest request)
        {
            string result = await mediator.Send(new GetButtonColorQuery { DeviceColor = request.DeviceToken });
            return Ok(new ExperimentResponse { Key = "button_color", Value = result });
        }

        [HttpGet]
        [Route("price")]
        public async Task<IActionResult> GetPrice([FromQuery] ExperimentRequest request)
        {
            int result = await mediator.Send(new GetPriceQuery { DeviceColor = request.DeviceToken });
            return Ok(new ExperimentResponse { Key = "price", Value = result.ToString() });
        }
    }
}
