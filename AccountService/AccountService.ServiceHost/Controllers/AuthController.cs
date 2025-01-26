using AccountService.Application.Handlers.RefreshToken;
using AccountService.ServiceHost.Controllers.Dto.Auth;
using AccountService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("refresh")]
    [Authorize("Refresh")]
    public async Task<ActionResult<GetTokenResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellation)
    {
        var token = GetTokenFormHeader();
        var command = new RefreshTokenCommand { ExpiredToken = token, RefreshToken = request.RefreshToken };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsFailure) return result.Error.ToErrorResult();

        return Ok(new GetTokenResponse { Token = result.Value.Token, RefreshToken = result.Value.RefreshToken });
    }

    [HttpGet("OnlyValidToken")]
    [Authorize]
    public async Task<ActionResult> Test1()
    {
        return Ok();
    }

    [HttpGet("OnlyUser")]
    [Authorize("User")]
    public async Task<ActionResult> Test2()
    {
        return Ok();
    }

    [HttpGet("OnlyAdmin")]
    [Authorize("Admin")]
    public async Task<ActionResult> Test3()
    {
        return Ok();
    }

    [HttpGet("ExpiredToken")]
    [Authorize("Refresh")]
    public async Task<ActionResult> Test4()
    {
        return Ok();
    }

    private string GetTokenFormHeader()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization)) return string.Empty;

        return ((string)Request.Headers.Authorization!).Replace("Bearer ", string.Empty);
    }
}
