using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.AssignToCase;
public class AssingToCaseCommand : IRequest<Result<AssingToCaseResult, Error>>
{
    public required int CaseId { get; init; }

    public required int ReviewerId { get; init; }
}
