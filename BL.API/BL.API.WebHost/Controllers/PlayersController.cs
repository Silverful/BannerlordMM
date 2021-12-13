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
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlayersController(IMediator mediator)
        {
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(string playerId)
        {
            var player = await _mediator.Send(new GetPlayerByIdQuery.Query(playerId));

            return Ok(player);
        }

        [HttpGet("{playerId}/stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlayerStats(string playerId)
        {
            var player = await _mediator.Send(new GetPlayerStats.Query(playerId));

            return Ok(player);
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

        [HttpPut("{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(string playerId, [FromBody] UpdatePlayerCommand request)
        {
            if (request.Id != playerId)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(request));
        }

        [HttpDelete("{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(string playerId)
        {
            return Ok(await _mediator.Send(new DeletePlayerCommand.Command(playerId)));
        }

        /// <summary>
        /// Partial update method. Body request example "[{"value": "Archer", "path": "/mainClass", "op": "replace"}]
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(string playerId, [FromBody] JsonPatchDocument<Player> request)
        {
            var player = await _mediator.Send(new GetPlayerByIdQuery.Query(playerId));

            request.ApplyTo(player, ModelState);

            var updateCmd = new UpdatePlayerCommand
            {
                Id = playerId,
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
