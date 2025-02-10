using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.GetCase;
public class GetCaseCommand : IRequest<Result<GetCaseResult?, Error>>
{
    public required int CaseId { get; init; }
}
