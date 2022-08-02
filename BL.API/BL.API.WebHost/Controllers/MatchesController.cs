using BL.API.Services.Matches.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/{regionShortName}/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator) : base()
        {
            _mediator = mediator;
        }

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<IEnumerable<MatchResponse>>> Get()
        //{
        //    var matchRecords = await _mediator.Send(new GetMatchesQuery.Query());
        //    return Ok(matchRecords);
        //}

        //[HttpGet("{matchId}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<MatchResponse>> GetById(Guid matchId)
        //{
        //    var matchRecords = await _mediator.Send(new GetMatchByIdQuery.Query(matchId));
        //    return Ok(matchRecords);
        //}

        [HttpPost]
        //[Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> Post(string regionShortName, [FromBody] UploadMatchCommand request)
        {
            request.RegionShortName = regionShortName;
            var matchId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = matchId }, matchId);
        }

        [HttpPut("{matchId}")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(string regionShortName, Guid matchId, [FromBody] UpdateMatchCommand request)
        {
            if (request.MatchId != matchId)
            {
                return BadRequest();
            }

            request.RegionShortName = regionShortName;

            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{matchId}")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid matchId)
        {
            await _mediator.Send(new DeleteMatchCommand.Query(matchId));
            return Ok();
        }
    }
}
