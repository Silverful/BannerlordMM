using BL.API.Services.Matches.Commands;
using BL.API.Services.Matches.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator) : base()
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MatchResponse>>> Get()
        {
            var matchRecords = await _mediator.Send(new GetMatchesQuery.Query());
            return Ok(matchRecords);
        }

        [HttpGet("{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MatchResponse>> GetById(Guid matchId)
        {
            var matchRecords = await _mediator.Send(new GetMatchByIdQuery.Query(matchId));
            return Ok(matchRecords);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> Post([FromBody] UploadMatchCommand request)
        {
            var matchId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = matchId }, matchId);
        }

        [HttpPut("{matchId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(Guid matchId, [FromBody] UpdateMatchCommand request)
        {
            if (request.MatchId != matchId)
            {
                return BadRequest();
            }

            await _mediator.Send(request);
            return Ok();
        }
    }
}
