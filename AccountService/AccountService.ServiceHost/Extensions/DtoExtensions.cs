using AccountService.Application.Handlers.GetAdmin;
using AccountService.Application.Handlers.GetUser;
using AccountService.Domain.Common;
using AccountService.ServiceHost.Controllers.Dto;
using AccountService.ServiceHost.Controllers.Dto.Admin;
using AccountService.ServiceHost.Controllers.Dto.RegisterUser;

namespace AccountService.ServiceHost.Extensions;

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

    public static GetUserResponse ToDto(this GetUserResult result)
    {
        return new GetUserResponse
        {
            Id = result.Id,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            BirthDate = result.BirthDate,
            Region = result.Region,
            Status = result.Status
        };
    }

    public static GetAdminResponse ToDto(this GetAdminResult result)
    {
        return new GetAdminResponse
        {
            Id = result.Id,
            Email = result.Email,
            FirtName = result.FirstName,
            LastName = result.LastName
        };
    }
}
