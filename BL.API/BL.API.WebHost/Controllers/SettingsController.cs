using BL.API.Services.Settings;
using BL.API.Services.Settings.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/{regionShortName}/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GetAllConfigsQuery.ConfigurationResponse>>> Get(string regionShortName)
        {
            return new JsonResult(await _mediator.Send(new GetAllConfigsQuery.Query(regionShortName)));
        }

        [HttpGet("{configName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetConfig(string regionShortName, string configName)
        {
            return new JsonResult(await _mediator.Send(new GetConfigQuery.Query(regionShortName, configName)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post(string regionShortName, [FromBody] AddConfigCommand request)
        {
            request.RegionShortName = regionShortName;
            var configId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = configId }, configId);
        }

        [HttpPut("{configName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(string regionShortName, string configName, [FromBody] UpdateConfigCommand request)
        {
            request.RegionShortName = regionShortName;
            if (configName != request.ConfigName) 
            {
                return BadRequest();
            };

            return Ok(await _mediator.Send(request));
        }
    }
}
