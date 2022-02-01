using BL.API.Services.Seasons.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeasonsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("StartNewSeason")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> StartNewSeason([FromBody] StartNewSeasonCommand request)
        {
            var seasonId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = seasonId }, seasonId);
        }
    }
}
