using AccountService.Application.Handlers.ChangeRegion;
using AccountService.Application.Handlers.CreateToken;
using AccountService.Application.Handlers.GetUser;
using AccountService.Application.Handlers.LoginUser;
using AccountService.Application.Handlers.RegisterUser;
using AccountService.Domain.Common;
using AccountService.ServiceHost.Controllers.Dto;
using AccountService.ServiceHost.Controllers.Dto.RegisterUser;
using AccountService.ServiceHost.Controllers.Dto.User;
using AccountService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
                AccountId = loginResult.Value.AccountId,
                UserId = loginResult.Value.UserId
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
    [Authorize]
    public async Task<ActionResult<PaginationWrapper<GetUserResponse>>> GetManyUsers(
        CancellationToken cancellation,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 1,
        [FromQuery] string? fullName = null,
        [FromQuery] AccountStatus? status = null)
    {
        var command = new GetManyUsersCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            FullName = fullName,
            Status = status
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var users = result.Value.GetPaginationResult(DtoExtensions.ToDto);

            return StatusCode(StatusCodes.Status200OK, users);
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
    
    [HttpPut("region")]
    [Authorize("User")]
    public async Task<ActionResult> UpdateRegion([FromBody] UpdateRegionRequest request, CancellationToken cancellation)
    {
        var userId = User.GetId();
        var command = new ChangeUserRegionCommand
        {
            UserId = (int)userId,
            region = request.region
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return Ok();

        return result.Error.ToErrorResult();
    }
}
