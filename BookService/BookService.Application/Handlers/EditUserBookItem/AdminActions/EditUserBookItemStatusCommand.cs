using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditUserBookItem.AdminActions;
public class EditUserBookItemStatusCommand : IRequest<Result<EditUserBookItemStatusResult, Error>>
{
    public required int UserBookItemId { get; init; }

    public required UserBookItemStatus Status { get; init; }
}
