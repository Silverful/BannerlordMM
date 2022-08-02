using BL.API.Core.Domain.Clan;
using BL.API.Services.Clans.Commands;
using BL.API.Services.Clans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BL.API.Services.Clans.Queries.GetClansList;
using static BL.API.Services.Clans.Queries.GetPendingRequests;

namespace BL.API.WebHost.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/{regionShortName}/[controller]")]
    public class ClansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClansController(IMediator mediator) : base()
        {
            _mediator = mediator;
        }

        [HttpGet("clansList")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ClanListItem>> GetClansList(string regionShortName)
        {
            var clanList = await _mediator.Send(new GetClansList.Query(regionShortName));

            return Ok(clanList);
        }

        [HttpGet("{clanId}/pending")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<PendingRequestsResponseItem>>> GetPendingRequests(string regionShortName, string clanId)
        {
            var requests = await _mediator.Send(new GetPendingRequests.Query(regionShortName, clanId));

            return Ok(requests);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> Post(string regionShortName, [FromBody] CreateClanCommand request)
        {
            request.RegionShortName = regionShortName;
            var clanId = await _mediator.Send(request);
            return CreatedAtAction("Post", new { id = clanId }, clanId.ToString());
        }

        [HttpPut("{clanId}")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(Guid clanId, [FromBody] UpdateClanCommand request)
        {
            if (request.ClanId != clanId)
            {
                return BadRequest();
            }

            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{clanId}")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid clanId)
        {
            await _mediator.Send(new DeleteClanCommand.Query(clanId));
            return Ok();
        }

        [HttpPatch("{clanId}")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(string clanId, [FromBody] JsonPatchDocument<Clan> request)
        {
            var clan = await _mediator.Send(new GetClanByIdQuery.Query(clanId));

            request.ApplyTo(clan, ModelState);

            var updateCmd = new UpdateClanCommand
            {
                ClanId = Guid.Parse(clanId),
                Name = clan.Name,
                Description = clan.Description,
                Color = clan.Color,
                LeaderId = clan.GetLeader().Id,
                AvatarURL = clan.AvatarURL,
                EnterType = clan.EnterType
            };

            return Ok(await _mediator.Send(updateCmd));
        }

        [HttpPost("{clanId}/join")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<JoinRequestClanResponse>> SendJoinRequest(string regionShortName, Guid clanId, [FromBody] JoinRequestClanCommand request)
        {
            if (clanId != request.RequestToClanId)
            {
                return BadRequest();
            }

            var req = await _mediator.Send(request);
            return CreatedAtAction("SendJoinRequest", req);
        }

        [HttpPost("{clanId}/request/{requestId}/approve")]
        [Authorize(Roles = "Admin,MatchMaker")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<JoinRequestClanResponse>> ApproveRequest(string regionShortName, Guid clanId, Guid requestId, [FromBody] ApproveRequestCommand request)
        {
            if (requestId != request.RequestId)
            {
                return BadRequest();
            }

            var req = await _mediator.Send(request);
            return Ok(req);
        }
    }
}
