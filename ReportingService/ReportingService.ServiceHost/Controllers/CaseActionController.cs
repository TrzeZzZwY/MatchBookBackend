using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportingService.Application.Handlers.ApproveCase;
using ReportingService.Application.Handlers.AssignToCase;
using ReportingService.Application.Handlers.CreateCase;
using ReportingService.Application.Handlers.GetCase;
using ReportingService.Application.Handlers.GetCasesList;
using ReportingService.Application.Handlers.RejectCase;
using ReportingService.Domain.Common;
using ReportingService.ServiceHost.Controllers.Dto;
using ReportingService.ServiceHost.Extenions;

namespace ReportingService.ServiceHost.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CaseActionController : ControllerBase
{
    private readonly IMediator _mediator;

    public CaseActionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateEntityResponse>> CreateCase([FromBody] CreateCaseRequest request, CancellationToken cancellation)
    {
        var command = new CreateCaseCommand
        {
            UserId = request.UserId,
            Type = request.Type,
            CaseFields = request.CaseFields,
            ItemId = request.ItemId,
            Notes = "Auto-created case",
            ReportType = ReportType.AutoReport
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return StatusCode(StatusCodes.Status201Created, new CreateEntityResponse { Id = result.Value.CaseId });

        return result.Error.ToErrorResult();
    }

    [HttpPost("report")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<CreateEntityResponse>> UserCreateCase([FromBody] CreateCaseRequestUser request, CancellationToken cancellation)
    {
        var command = new CreateCaseCommand
        {
            UserId = request.UserId,
            Type = request.Type,
            CaseFields = request.CaseFields,
            ItemId = request.ItemId,
            ReportType = ReportType.UserReport,
            Notes = request.Notes
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return StatusCode(StatusCodes.Status201Created, new CreateEntityResponse { Id = result.Value.CaseId });

        return result.Error.ToErrorResult();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PaginationWrapper<CaseItemListResponse>>> GetCases(CancellationToken cancellation,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 1,
        [FromQuery] CaseStatus? caseStatus = null,
        [FromQuery] CaseItemType? caseType = null,
        [FromQuery] int? userId = null)
    {
        var command = new GetCasesListCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            CaseStatus = caseStatus,
            CaseType = caseType,
            UserId = userId
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var books = result.Value.GetPaginationResult(DtoExtensions.ToDto);
            return StatusCode(StatusCodes.Status200OK, books);
        };

        return result.Error.ToErrorResult();
    }

    [HttpGet("{caseId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CaseResponse>> GetCase(CancellationToken cancellation,
        [FromRoute] int caseId)
    {
        var command = new GetCaseCommand
        {
            CaseId = caseId
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status200OK, result.Value.ToDto());
        };

        return result.Error.ToErrorResult();
    }

    [HttpPut("{caseId:int}/Assign")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AssignToCase(CancellationToken cancellation, [FromRoute] int caseId)
    {
        var command = new AssingToCaseCommand
        {
            CaseId = caseId,
            ReviewerId = (int)User.GetId()
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return Ok();

        return result.Error.ToErrorResult();
    }

    [HttpPut("{caseId:int}/Approve")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ApproveCase(CancellationToken cancellation, [FromRoute] int caseId)
    {
        var command = new ApproveCaseCommand
        {
            CaseId = caseId,
            ReviewerId = (int)User.GetId()
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return Ok();

        return result.Error.ToErrorResult();
    }

    [HttpPut("{caseId:int}/Reject")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RejectCase(CancellationToken cancellation, [FromRoute] int caseId)
    {
        var command = new RejectCaseCommand
        {
            CaseId = caseId,
            ReviewerId = (int)User.GetId()
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return Ok();

        return result.Error.ToErrorResult();
    }
}
