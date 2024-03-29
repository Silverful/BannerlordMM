﻿using BL.API.Services.Seasons.Commands;
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
    public class SeasonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SeasonsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("StartNewSeason")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> StartNewSeason([FromBody] StartNewSeasonCommand request)
        {
            var seasonId = await _mediator.Send(request);
            return CreatedAtAction("StartNewSeason", new { id = seasonId }, seasonId);
        }

        [HttpDelete("DeleteCurrentSeason")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCurrentSeason()
        {
            await _mediator.Send(new DeleteCurrentSeasonCommand());
            return Ok();
        }
    }
}
