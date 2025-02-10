using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.GetCasesList;
public class GetCasesListCommand : IRequest<Result<PaginatedResult<GetCasesListItem>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public int? UserId { get; init; }

    public CaseStatus? CaseStatus { get; init; }

    public CaseItemType? CaseType { get; init; }
}
