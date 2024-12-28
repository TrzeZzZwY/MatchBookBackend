using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetUserBookItemCommand : IRequest<Result<GetUserBookResult?, Error>>
{
    public required int UserBookItemId { get; set; }
}
