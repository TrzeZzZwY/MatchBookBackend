using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditUserBookItem;
public class EditUserBookItemCommand : IRequest<Result<EditUserBookItemResult, Error>>
{
    public required int UserBookItemId { get; init; }

    public required int UserId { get; init; }

    public required string Description { get; init; }

    public required UserBookItemStatus Status { get; init; }

    public required int BookReferenceId { get; init; }

    public int? BookPointId { get; init; }

    public int? ImageId { get; init; }

    public required Region Region { get; init; }
}