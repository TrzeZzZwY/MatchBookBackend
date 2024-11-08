using AccountService.Application.Handlers.GetUser;
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
            Password = request.Password
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess) return StatusCode(StatusCodes.Status201Created);

        return result.Error.Reason switch
        {
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
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

        var result = await _mediator.Send(command, new());

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

        return result.Error.Reason switch
        {
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<GetUserResponse>> GetManyUsers([FromRoute] int userId, CancellationToken cancellation)
    {
        var command = new GetUserCommand
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
