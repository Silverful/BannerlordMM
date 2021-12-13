using BL.API.Services.Matches.Commands;
using BL.API.Services.Matches.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> Get()
        {
            var matchRecords = await _mediator.Send(new GetMatchesQuery.Query());
            return Ok(matchRecords);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] UploadMatchCommand request)
        {
            var matchId = await _mediator.Send(request);
            return CreatedAtAction(nameof(UploadMatchCommand), new { id = matchId }, matchId);
        }
    }
}
