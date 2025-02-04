using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBook;
public class GetManyBooksCommand : IRequest<Result<PaginatedResult<GetBookResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public string? Title { get; init; }

    public bool InludeAuthorDetails { get; init; } = false;

    public bool ShowRemoved { get; set; } = false;

    public int? AuthorId { get; set; } = null;
}
