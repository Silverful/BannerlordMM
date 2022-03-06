using BL.API.Services.Authorization.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BL.API.WebHost.Controllers
{
    [ApiController]
    [Route("api/{regionShortName}/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            await _mediator.Send(request);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("registeradmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminCommand request)
        {
            await _mediator.Send(request);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("addrole")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserCommand request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
