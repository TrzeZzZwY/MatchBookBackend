using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.RejectCase;
public class RejectCaseCommand : IRequest<Result<RejectCaseResult, Error>>
{
    public required int CaseId { get; init; }

    public required int ReviewerId { get; init; }
}
