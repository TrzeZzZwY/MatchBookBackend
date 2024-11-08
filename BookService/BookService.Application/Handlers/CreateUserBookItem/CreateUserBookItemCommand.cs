using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateUserBookItem;
public class CreateUserBookItemCommand : IRequest<Result<CreateUserBookItemResult, Error>>
{
    public required int UserId { get; set; }

    public required string Description { get; set; }

    public required UserBookItemStatus Status { get; set; }

    public required int BookReferenceId { get; set; }

    public int? BookPointId { get; set; }
}
