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
        [Route("fill-600-color")]
        public async Task<IActionResult> GetButtonColorAsync()
        {
            try
            {
                for(int i = 1; i <= 600; i++)
                {
                    var result = await mediator.Send(new GetButtonColorQuery { DeviceToken = $"test{i}" });
                }
                
                return Ok("DB Fill");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("button-color")]
        public async Task<IActionResult> GetButtonColorAsync([FromQuery] ExperimentRequest request)
        {
            try
            {
                var result = await mediator.Send(new GetButtonColorQuery { DeviceToken = request.DeviceToken });
                return Ok(new ShortExperimentResponse { Key = "button_color", Value = result });
            }  catch(Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("price")]
        public async Task<IActionResult> GetPrice([FromQuery] ExperimentRequest request)
        {
            try
            {
                var result = await mediator.Send(new GetPriceQuery { DeviceToken = request.DeviceToken });
                return Ok(new ShortExperimentResponse { Key = "price", Value = result.ToString() });
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("statistics")]
        public async Task<IActionResult> GetStatistics()
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
