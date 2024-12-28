using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteAuthor;
public class DeleteAuthorCommand : IRequest<Result<DeleteAuthorResult, Error>>
{
    public required int AuthorId { get; init; }
}