using MatchBook.App.Command.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http;

namespace MatchBook.WebApi.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand registration)
    {
      await _mediator.Send(registration);
      return Ok();
    }
}
