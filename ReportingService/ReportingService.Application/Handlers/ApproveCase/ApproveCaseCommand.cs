using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.ApproveCase;
public class ApproveCaseCommand : IRequest<Result<ApproveCaseResult, Error>>
{
    public required int CaseId { get; init; }

    public required int ReviewerId { get; init; }
}
