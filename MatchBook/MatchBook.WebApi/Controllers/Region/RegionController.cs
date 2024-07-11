using MatchBook.App.Command.CreateRegion;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MatchBook.WebApi.Controllers.Region;

[Route("api/[controller]")]
[ApiController]
public class RegionController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}
