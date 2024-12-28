using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditBook;
public class EditBookCommand : IRequest<Result<EditBookResult, Error>>
{
    public required int BookId { get; set; }

    public required string Title { get; init; }

    public List<int>? AuthorsId { get; init; }
}
