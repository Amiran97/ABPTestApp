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

        [HttpGet]
        [Route("button-color")]
        public async Task<IActionResult> GetButtonColorAsync([FromQuery] ExperimentRequest request)
        {
            try
            {
                var result = await mediator.Send(new GetButtonColorQuery { DeviceToken = request.DeviceToken });
                return Ok(new ExperimentResponse { Key = "button_color", Value = result });
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
                return Ok(new ExperimentResponse { Key = "price", Value = result.ToString() });
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await mediator.Send(new GetAllQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
