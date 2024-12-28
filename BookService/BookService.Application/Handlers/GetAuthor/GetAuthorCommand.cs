using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetAuthor;
public class GetAuthorCommand : IRequest<Result<GetAuthorResult?, Error>>
{
    public int AuthorId { get; init; }
}
