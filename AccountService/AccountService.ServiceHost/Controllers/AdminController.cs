using AccountService.Application.Handlers.CreateToken;
using AccountService.Application.Handlers.GetAdmin;
using AccountService.Application.Handlers.LoginAdmin;
using AccountService.Application.Handlers.RegisterAdmin;
using AccountService.Domain.Common;
using AccountService.ServiceHost.Controllers.Dto;
using AccountService.ServiceHost.Controllers.Dto.Admin;
using AccountService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("Register")]
    [Authorize("Admin")]
    public async Task<ActionResult> Register([FromBody] RegisterAdminRequest request, CancellationToken cancellation)
    {
        var userId = User.GetId();
        if (userId is null) return StatusCode(StatusCodes.Status400BadRequest);

        var command = new RegisterAdminCommand
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            CreatedBy = (int)userId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess) return StatusCode(StatusCodes.Status201Created);

        return result.Error.Reason switch
        {
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginAdminRequest request, CancellationToken cancellation)
    {
        var loginCommand = new LoginAdminCommand
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
            if (getTokenResult.IsSuccess)
                return StatusCode(StatusCodes.Status201Created,
                    new TokenResponse { Token = getTokenResult.Value.Token, RefreshToken = getTokenResult.Value.RefreshToken });

            return getTokenResult.Error.ToErrorResult();
        }

        return loginResult.Error.ToErrorResult();
    }

    [HttpGet]
    [Authorize("Admin")]
    public async Task<ActionResult<PaginationWrapper<GetAdminResponse>>> GetManyAdmins(CancellationToken cancellation, [FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1)
    {
        var command = new GetManyAdminsCommand
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
            return Ok(new PaginationWrapper<GetAdminResponse>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = users,
                ItemsCount = users.Count()
            });
        }

        return result.Error.Reason switch
        {
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpGet("{userId:int}")]
    [Authorize("Admin")]
    public async Task<ActionResult<GetAdminResponse>> GetAdmin([FromRoute] int userId, CancellationToken cancellation)
    {
        var command = new GetAdminCommand
        {
            Id = userId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return Ok(result.Value.ToDto());


        return result.Error.Reason switch
        {
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
