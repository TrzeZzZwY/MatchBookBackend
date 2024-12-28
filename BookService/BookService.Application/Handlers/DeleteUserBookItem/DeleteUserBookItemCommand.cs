using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteUserBookItem;
public class DeleteUserBookItemCommand : IRequest<Result<DeleteUserBookItemResult, Error>>
{
    public required int UserBookItemId { get; set; }
}
