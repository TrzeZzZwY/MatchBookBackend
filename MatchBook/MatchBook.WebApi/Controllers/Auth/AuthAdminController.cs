using MatchBook.App.Command.CreateUser;
using MatchBook.App.Query.RefreshToken;
using MatchBook.App.Query.SignIn;
using MatchBook.WebApi.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchBook.WebApi.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] SignInRequest request)
        {
            try
            {
                var response = await _mediator.Send(
                    new SignInQuery
                    {
                        Login = request.Login,
                        Password = request.Password,
                        Role = Domain.Enums.ApplicationRole.admin
                    });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] CreateAdminCommand request)
        {
            try
            {
                await _mediator.Send(request);
                var response = await _mediator.Send(
                    new SignInQuery 
                    { 
                        Login = request.Email,
                        Password = request.Password,
                        Role = Domain.Enums.ApplicationRole.admin
                    });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenQuery request)
        {
            try
            {
                var response = await _mediator.Send(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Authorize")]
        public async Task<IActionResult> Authorize()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
