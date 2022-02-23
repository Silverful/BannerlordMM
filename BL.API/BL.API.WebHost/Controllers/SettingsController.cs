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
        public async Task<ActionResult<IEnumerable<GetAllConfigsQuery.ConfigurationResponse>>> Get()
        {
            return new JsonResult(await _mediator.Send(new GetAllConfigsQuery.Query()));
        }

        [HttpGet("{configName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetConfig(string configName)
        {
            return new JsonResult(await _mediator.Send(new GetConfigQuery.Query(configName)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] AddConfigCommand request)
        {
            var configId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = configId }, configId);
        }

        [HttpPut("{configName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(string configName, [FromBody] UpdateConfigCommand request)
        {
            if (configName != request.ConfigName) 
            {
                return BadRequest();
            };

            return Ok(await _mediator.Send(request));
        }
    }
}
