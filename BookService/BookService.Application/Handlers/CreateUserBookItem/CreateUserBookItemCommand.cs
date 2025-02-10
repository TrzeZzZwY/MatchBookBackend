using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateUserBookItem;
public class CreateUserBookItemCommand : IRequest<Result<CreateUserBookItemResult, Error>>
{
    public required int UserId { get; init; }

    public required string Description { get; init; }

    public required UserBookItemStatus Status { get; init; }

    public required int BookReferenceId { get; init; }

    public int? BookPointId { get; init; }

    public int? ImageId { get; init; }
}
