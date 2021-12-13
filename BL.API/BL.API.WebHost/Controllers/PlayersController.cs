using BL.API.Core.Domain.Player;
using BL.API.Services.Players.Commands;
using BL.API.Services.Players.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllPlayersQuery.Query()));
        }

        [HttpGet("{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromQuery] string playerId)
        {
            if (!Guid.TryParse(playerId, out Guid id)) return BadRequest();

            var player = await _mediator.Send(new GetPlayerByIdQuery.Query(id));

            return player == null ? NotFound() : Ok(player);
        }

        [HttpGet("{playerId}/stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] AddPlayerCommand request)
        {
            var player = await _mediator.Send(request);
            return CreatedAtAction(nameof(AddPlayerCommand), new { id = player }, player);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] UpdatePlayerCommand request, string playerId)
        {
            if (!Guid.TryParse(playerId, out Guid id)) return BadRequest();

            var dbPlayer = await _mediator.Send(new GetPlayerByIdQuery.Query(id));

            if (dbPlayer == null)
                return NotFound();

            return Ok(await _mediator.Send(request));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete([FromQuery] string playerId)
        {
            if (!Guid.TryParse(playerId, out Guid id)) return BadRequest();

            var player = await _mediator.Send(new GetPlayerByIdQuery.Query(id));

            if (player == null)
                return NotFound();

            return Ok(await _mediator.Send(new DeletePlayerCommand.Command(id)));
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<Player> request, string playerId)
        {
            if (!Guid.TryParse(playerId, out Guid id)) return BadRequest();

            var player = await _mediator.Send(new GetPlayerByIdQuery.Query(id));

            if (player == null)
                return NotFound();

            request.ApplyTo(player, ModelState);

            var updateCmd = new UpdatePlayerCommand
            {
                Id = player.Id,
                Nickname = player.Nickname,
                Country = player.Country,
                Clan = player.Clan,
                MainClass = player.MainClass.ToString(),
                SecondaryClass = player.SecondaryClass.ToString(),
                DiscordId = player.DiscordId,
                PlayerMMR = player.PlayerMMR
            };

            return Ok(await _mediator.Send(updateCmd));
        }
    }
}
