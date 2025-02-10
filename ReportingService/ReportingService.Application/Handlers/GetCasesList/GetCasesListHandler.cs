using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Application.Extensions;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.GetCasesList;
public class GetCasesListHandler : IRequestHandler<GetCasesListCommand, Result<PaginatedResult<GetCasesListItem>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetCasesListHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetCasesListItem>, Error>> Handle(GetCasesListCommand request, CancellationToken cancellationToken)
    {
        var cases = _databaseContext.Cases.AsQueryable();

        if (request.UserId is not null)
            cases = cases.Where(e => e.UserId == request.UserId);

        if (request.CaseStatus is not null)
            cases = cases.Where(e => e.CaseStatus == request.CaseStatus);

        if (request.CaseType is not null)
            cases = cases.Where(e => e.CaseItemType == request.CaseType);

        var total = cases.Count();

        cases = cases
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return request.PaginationOptions.ToPaginatedResult(cases.ToList().Select(e =>
        new GetCasesListItem
        {
            CaseId = e.Id,
            CaseType = e.CaseItemType,
            CaseStatus = e.CaseStatus,
            UserId = e.UserId,
            ReportType = e.ReportType,
            ReviewerId = e.ReviewerId
        }).ToList()
        , total);
    }
}
