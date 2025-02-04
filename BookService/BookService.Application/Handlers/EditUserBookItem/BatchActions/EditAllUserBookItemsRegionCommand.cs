using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditUserBookItem.BatchActions;
public class EditAllUserBookItemsRegionCommand : IRequest<Result<EditAllUserBookItemsRegionResult, Error>>
{
    public required int UserId { get; init; }

    public required Region Region { get; init; }
}
