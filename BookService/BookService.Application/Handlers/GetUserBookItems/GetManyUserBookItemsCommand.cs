using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetManyUserBookItemsCommand : IRequest<Result<List<GetUserBookResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }
}
