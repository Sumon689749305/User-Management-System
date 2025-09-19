using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Application.Features.SendEmail.Commands;

namespace UserManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SendEmailController> _logger;
        public SendEmailController(ILogger<SendEmailController> logger,IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailCommand sendEmailCommand)
        {
            var result = await _mediator.Send(sendEmailCommand);
            _logger.LogInformation("Email sent successfully");
            return Ok(new { Status = result });
        }
    }
}
