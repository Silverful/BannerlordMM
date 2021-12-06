using BL.API.Core.Domain.Player;
using BL.API.Services.Players.Commands;
using BL.API.Services.Players.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IMediator _mediator;

        public PlayersController(ILogger<PlayersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllPlayersQuery.Query()));
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetById([FromQuery] string playerId)
        {
            if (!Guid.TryParse(playerId, out Guid id)) return BadRequest();

            var player = await _mediator.Send(new GetPlayerByIdQuery.Query(id));

            return player == null ? NotFound() : Ok(player);
        }

        [HttpGet("{playerId}/stats")]
        public async Task<IActionResult> GetPlayerStats([FromQuery] string playerId)
        {
            if (!Guid.TryParse(playerId, out Guid id)) return BadRequest();

            var player = await _mediator.Send(new GetPlayerStats.Query(id));

            return player == null ? NotFound() : Ok(player);
        }


        [HttpGet("/stats")]
        public async Task<IActionResult> GetPlayersStats()
        {
            return Ok(await _mediator.Send(new GetPlayersStats.Query()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Player player)
        {
            return Ok(await _mediator.Send(new AddPlayerCommand.Command(player)));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Player player)
        {
            return Ok(await _mediator.Send(new UpdatePlayerCommand.Command(player)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string playerId)
        {
            if (!Guid.TryParse(playerId, out Guid id)) return BadRequest();

            return Ok(await _mediator.Send(new DeletePlayerCommand.Command(id)));
        }
    }
}
