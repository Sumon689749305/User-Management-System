using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Application.Features.SendEmail;

namespace UserManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SendEmailController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailCommand sendEmailCommand)
        {
            var result = await _mediator.Send(sendEmailCommand);
            return Ok(new { Status = result });
        }
    }
}
