using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetUserLikes;
public class GetUserLikesHandler : IRequestHandler<GetUserLikesCommand, Result<GetUserLikesResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetUserLikesHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetUserLikesResult, Error>> Handle(GetUserLikesCommand request, CancellationToken cancellationToken)
    {
        var itemsId = _databaseContext.UserLikesBooks.Where(e => e.UserId == request.UserId).Select(e => e.UserBookItemId);

        var items = _databaseContext.UserBookItems.Where(e => itemsId.Contains(e.Id));

        var total = items.Count();
        items = items
        .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
        .Take(request.PaginationOptions.PageSize);

        items = items.Include(e => e.BookReference);

        return new GetUserLikesResult
        {
            UserId = request.UserId,
            Items = request.PaginationOptions.ToPaginatedResult(items.ToList().Select(e => e.ToHandlerResult()).ToList(), total)
        };
    }
}
