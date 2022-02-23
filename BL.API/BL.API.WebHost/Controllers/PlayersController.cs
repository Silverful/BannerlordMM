using BL.API.Core.Domain.Player;
using BL.API.Services.Players.Commands;
using BL.API.Services.Players.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using BL.API.Services.Stats.Model;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/{regionShortName}/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlayersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Player>>> Get()
        {
            return Ok(await _mediator.Send(new GetAllPlayersQuery.Query()));
        }

        /// <summary>
        /// Gets player by GUID
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Player>> GetById(string playerId)
        {
            var player = await _mediator.Send(new GetPlayerByIdQuery.Query(playerId));

            return Ok(player);
        }

        /// <summary>
        /// Get players stats by GUID
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("{playerId}/stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlayerStatItemResponse>> GetPlayerStats(string regionShortName, [FromRoute]string playerId)
        {
            var player = await _mediator.Send(new GetPlayerStatsQuery.Query(playerId, regionShortName));

            return Ok(player);
        }

        /// <summary>
        /// Gets players stats by DiscordId
        /// </summary>
        /// <param name="discordId"></param>
        /// <returns></returns>
        [HttpGet("discord/{discordId}/stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlayerStatItemResponse>> GetPlayerStatsByDiscordId(string regionShortName, [FromRoute]long discordId)
        {
            var player = await _mediator.Send(new GetPlayerStatsByDiscordIdQuery.Query(discordId, regionShortName));

            return Ok(player);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<IEnumerable<PlayerStatItemResponse>>> GetPlayersStats(string regionShortName)
        {
            return Ok(await _mediator.Send(new GetPlayersStatsQuery.Query(null, null, null, regionShortName)));
        }

        [HttpGet("nicknames")]
        public async Task<ActionResult<GetNicknamesQuery.PlayerNickname>> GetNicknames()
        {
            return Ok(await _mediator.Send(new GetNicknamesQuery.Query()));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] AddPlayerCommand request)
        {
            var playerId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = playerId }, playerId);
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
        /// Partial update method. Body request example: [{"value": "Archer", "path": "/mainClass", "op": "replace"}]
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
                IGL = player.IsIGL,
                Country = player.Country,
                Clan = player.Clan,
                MainClass = player.MainClass.ToString(),
                SecondaryClass = player.SecondaryClass.ToString(),
                DiscordId = player.DiscordId
            };

            return Ok(await _mediator.Send(updateCmd));
        }
    }
}
