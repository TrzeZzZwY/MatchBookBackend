using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetAllUserItemsCommand : IRequest<Result<PaginatedResult<GetUserBookResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public int UserId { get; init; }

    public string? Title { get; init; } = null;

    public int RequestUserId { get; init; }
}
