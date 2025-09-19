using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Application.Features.Authentication.Commands;

namespace UserManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<string> Login([FromBody]LoginCommand loginCommand)
        {
            var token = await _mediator.Send(loginCommand);
            _logger.LogInformation("User logged in successfully");
            return token;
        }

    }
}
