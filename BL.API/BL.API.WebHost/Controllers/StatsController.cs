using BL.API.Services.Players.Queries;
using BL.API.Services.Stats.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/{regionShortName}/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AllStatsResponse>> GetAllStats(string regionShortName)
        {
            return Ok(await _mediator.Send(new GetAllStatsQuery.Query(regionShortName)));
        }
    }
}
