using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Controllers.Dto.Author;
using BookService.ServiceHost.Controllers.Dto.Book;
using BookService.ServiceHost.Controllers.Dto.BookPoint;
using BookService.ServiceHost.Controllers.Dto.UserItemBook;
using BookService.Domain.Common;
using BookService.Application.Handlers.GetAuthor;
using BookService.Application.Handlers.GetBook;
using BookService.Application.Handlers.GetBookPoint;
using BookService.Application.Handlers.GetUserBookItems;

namespace BookService.ServiceHost.Extensions;

public static class DtoExtensions
{
    public static PaginationWrapper<E> GetPaginationResult<T, E>(this PaginatedResult<T> paginatedResult, Func<T,E> mapper)
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

    public static AuthorResponse ToDto(this GetAuthorResult result)
    {
        return new AuthorResponse
        {
            Id = result.Id,
            FirstName = result.FistName,
            LastName = result.LastName,
            YearOfBirth = result.YearOfBirth,
            Country = result.Country,
            IsRemoved = result.IsRemoved
        };
    }

    public static BookResponse ToDto(this GetBookResult result)
    {
        return new BookResponse
        {
            Id = result.Id,
            Title = result.Title,
            Authors = result.Authors?.Select(a => a.ToDto()),
            IsRemoved = result.IsRemoved
        };
    }

    public static BookPointResponse ToDto(this GetBookPointResult result)
    {
        return new BookPointResponse
        {
            Id = result.Id,
            Lat = result.Lat,
            Long = result.Long,
            Region = result.Region,
            Capacity = result.Capacity
        };
    }

    public static UserBookItemResponse ToDto(this GetUserBookResult result)
    {
        return new UserBookItemResponse
        {
            Id = result.Id,
            UserId = result.UserId,
            Description = result.Description,
            Status = result.Status,
            BookReference = result.BookReference.ToDto(),
            ImageId = result.ImageId
        };
    }
}
