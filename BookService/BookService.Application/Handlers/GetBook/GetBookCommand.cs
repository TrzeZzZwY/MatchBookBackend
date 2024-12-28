using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBook;
public class GetBookCommand : IRequest<Result<GetBookResult?, Error>>
{
    public required int BookId { get; init; }

    public bool InludeAuthorDetails { get; init; } = false;
}