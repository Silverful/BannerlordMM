using BL.API.Services.Regions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/{regionShortName}/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] AddRegionCommand request)
        {
            var regionId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = regionId }, regionId);
        }
    }
}