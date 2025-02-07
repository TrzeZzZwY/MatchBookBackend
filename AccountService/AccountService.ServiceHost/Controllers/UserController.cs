using AccountService.Application.Handlers.CreateToken;
using AccountService.Application.Handlers.GetUser;
using AccountService.Application.Handlers.LoginUser;
using AccountService.Application.Handlers.RegisterUser;
using AccountService.Domain.Common;
using AccountService.ServiceHost.Controllers.Dto;
using AccountService.ServiceHost.Controllers.Dto.RegisterUser;
using AccountService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellation)
    {
        var command = new RegisterUserCommand
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            BirthDate = request.BithDate,
            Region = request.Region
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess) return StatusCode(StatusCodes.Status201Created);

        return result.Error.ToErrorResult();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginUserRequest request, CancellationToken cancellation)
    {
        var loginCommand = new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var loginResult = await _mediator.Send(loginCommand, cancellation);

        if (loginResult.IsSuccess)
        {
            var getTokenCommand = new CreateTokenCommand
            {
                AccountId = loginResult.Value.AccountId
            };

            var getTokenResult = await _mediator.Send(getTokenCommand, cancellation);
            if(getTokenResult.IsSuccess)
                return StatusCode(StatusCodes.Status201Created,
                    new TokenResponse { Token = getTokenResult.Value.Token, RefreshToken = getTokenResult.Value.RefreshToken });
            return getTokenResult.Error.ToErrorResult();
        }

        return loginResult.Error.ToErrorResult();
    }

    [HttpGet]
    public async Task<ActionResult<PaginationWrapper<GetUserResponse>>> GetManyUsers(CancellationToken cancellation, [FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1)
    {
        var command = new GetManyUsersCommand
        {
            paginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            }
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var users = result.Value.Select(e => e.ToDto());
            return Ok(new PaginationWrapper<GetUserResponse>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = users,
                ItemsCount = users.Count()
            });
        }

        return result.Error.ToErrorResult();
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<GetUserResponse>> GetUser([FromRoute] int userId, CancellationToken cancellation)
    {
        var command = new GetUserCommand
        {
            Id = userId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return Ok(result.Value.ToDto());

        return result.Error.ToErrorResult();
    }
}
