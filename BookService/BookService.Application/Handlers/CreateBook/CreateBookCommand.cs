using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateBook;
public class CreateBookCommand : IRequest<Result<CreateBookResult, Error>>
{
    public required string Title { get; init; }

    public List<int>? AuthorsId { get; init; }
}
