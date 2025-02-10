using ReportingService.Application.Handlers.GetCase;
using ReportingService.Application.Handlers.GetCasesList;
using ReportingService.Domain.Common;
using ReportingService.ServiceHost.Controllers.Dto;

namespace ReportingService.ServiceHost.Extenions;

public static class DtoExtensions
{
    public static PaginationWrapper<E> GetPaginationResult<T, E>(this PaginatedResult<T> paginatedResult, Func<T, E> mapper)
    {
        return new PaginationWrapper<E>
        {
            Items = paginatedResult.Items.Select(e => mapper(e)),
            ItemsCount = paginatedResult.Items.Count(),
            PageNumber = paginatedResult.PageNumber,
            PageSize = paginatedResult.PageSize,
            TotalItemsCount = paginatedResult.TotalItemsCount
        };
    }

    public static CaseItemListResponse ToDto(this GetCasesListItem result)
    {
        return new CaseItemListResponse
        {
            CaseId = result.CaseId,
            UserId = result.UserId,
            CaseStatus = result.CaseStatus,
            CaseType= result.CaseType,
            ReportType = result.ReportType,
            ReviewerId = result.ReviewerId
        };
    }

    public static CaseResponse ToDto(this GetCaseResult result) 
    {
        return new CaseResponse
        {
            CaseId = result.CaseId,
            UserId = result.UserId,
            CaseStatus = result.CaseStatus,
            CaseType = result.CaseType,
            keyValues = result.keyValues,
            ItemId = result.ItemId,
            ReportNote = result.ReportNote,
            ReportType = result.ReportType,
            ReviewerId = result.ReviewerId
        };
    }
}
