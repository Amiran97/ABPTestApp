using ABPTestApp.Domains.Experiment.Queries;
using ABPTestApp.Models.DTOs;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Data.SqlClient;

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


        /* 
        This method is not required in the test job.
        But I decided to add it to automatically fill the DB!
        */
        [HttpGet]
        [Route("fill-600-devices")]
        public async Task<IActionResult> GetFillAsync()
        {
            try
            {
                for(int i = 1; i <= 600; i++)
                {
                    await mediator.Send(new GetExperimentQuery { DeviceToken = $"test{i}", Key = "button-color" });
                    await mediator.Send(new GetExperimentQuery { DeviceToken = $"test{i}", Key = "price" });
                }
                
                return Ok("DB Fill");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> GetExperimentAsync([FromRoute] string key, [FromQuery] ExperimentRequest request)
        {
            if(!string.IsNullOrWhiteSpace(key) && (key.Equals("price") || key.Equals("button-color")))
            {
                try
                {
                    var result = await mediator.Send(new GetExperimentQuery { DeviceToken = request.DeviceToken, Key = key });
                    return Ok(new ShortExperimentResponse { Key = key, Value = result });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            } else
            {
                return BadRequest("Invalid key! Key must be 'button-color' or 'price'");
            }
        }

        [HttpGet]
        [Route("statistics")]
        public async Task<IActionResult> GetStatisticsAsync()
        {
            try
            {
                var result = await mediator.Send(new GetStatisticsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
