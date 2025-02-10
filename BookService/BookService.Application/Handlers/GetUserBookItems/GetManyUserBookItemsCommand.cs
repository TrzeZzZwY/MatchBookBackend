using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetManyUserBookItemsCommand : IRequest<Result<PaginatedResult<GetUserBookResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public bool InludeAuthorDetails { get; init; } = false;

    public UserBookItemStatus? ItemStatus { get; init; } = null;

    public Region? Region { get; init; } = null;

    public int? UserId { get; init; } = null;

    public string? Title { get; init; } = null;

    public DateTime? StartDate { get; init; } = null;

    public DateTime? EndDate { get; init; } = null;
}
