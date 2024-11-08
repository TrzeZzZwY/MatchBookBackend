using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBook;
public class GetManyBooksCommand: IRequest<Result<List<GetBookResult>,Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public string? Title { get; init; }

    public bool InludeAuthorDetails { get; init; } = false;
}
