using AccountService.Application.Handlers.UserStatus;
using AccountService.ServiceHost.Controllers.Dto.UserManagment;
using AccountService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.ServiceHost.Controllers;

public class UserManagmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserManagmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("account-status")]
    [Authorize("Admin")]
    public async Task<ActionResult> ChangeUserStatus([FromBody] ChangeUserStatusRequest request, CancellationToken cancellation)
    {
        var command = new ChangeUserStatusCommand { Status = request.Status, UserId = request.UserId };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess) return Ok();

        return result.Error.ToErrorResult();
    }
}
