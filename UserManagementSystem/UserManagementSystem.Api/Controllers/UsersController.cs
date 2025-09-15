using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Application.Features.Users.Queries;

namespace UserManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        public UsersController(ILogger<UsersController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _mediator.Send(new GetUserQuery()));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(UserAddCommand userAddCommand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(userAddCommand);
                    return Ok(new { Message = "User created successfully" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add User");
                    return BadRequest(ex);
                }
            }
            return Ok(userAddCommand);
        }

        //    [HttpGet("id")]
        //    public async Task<IActionResult> GetAProductData(int id)
        //    {
        //        return Ok(await _productService.GetProductById(id));
        //    }
        [HttpGet("id")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var model = new UserUpdateCommand();
            var employee = await _mediator.Send(new GetUpdateUserQuery(id));
            _mapper.Map(employee, model);
            return Ok(model);
        }
        [HttpPost("id")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateCommand userUpdateCommand)
        {
            userUpdateCommand.Id = id;
            await _mediator.Send(userUpdateCommand);

            return Ok(new { Message = "User updated successfully" });
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id, UserDeleteCommand userDeleteCommand)
        {
            userDeleteCommand.Id = id;
            await _mediator.Send(userDeleteCommand);
            return Ok(new { Message = "User Delete successfully" });
        }
    }
}
