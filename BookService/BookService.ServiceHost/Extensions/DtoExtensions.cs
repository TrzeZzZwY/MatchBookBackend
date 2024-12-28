using BookService.Application.Handlers.GetBook;
using BookService.Application.Handlers.GetAuthor;
using BookService.Application.Handlers.GetBookPoint;
using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Controllers.Dto.Author;
using BookService.ServiceHost.Controllers.Dto.Book;
using BookService.ServiceHost.Controllers.Dto.BookPoint;
using BookService.ServiceHost.Controllers.Dto.UserItemBook;
using BookService.Application.Handlers.GetUserBookItems;

namespace BookService.ServiceHost.Extensions;

public static class DtoExtensions
{
    public static PaginationWrapper<T> GetPaginationResult<T>(this IEnumerable<T> collection, int PageNumber, int PageSize)
    {
        return new PaginationWrapper<T>
        {
            Items = collection,
            ItemsCount = collection.Count(),
            PageNumber = PageNumber,
            PageSize = PageSize
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
        };
    }
}
