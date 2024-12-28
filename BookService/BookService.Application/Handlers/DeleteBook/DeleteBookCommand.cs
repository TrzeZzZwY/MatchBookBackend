using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteBook;
public class DeleteBookCommand : IRequest<Result<DeleteBookResult, Error>>
{
    public required int BookId { get; set; }
}
