using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetAllUserItemsHandler : IRequestHandler<GetAllUserItemsCommand, Result<PaginatedResult<GetUserBookResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetAllUserItemsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetUserBookResult>, Error>> Handle(GetAllUserItemsCommand request, CancellationToken cancellationToken)
    {
        var userBooks = _databaseContext.UserBookItems.AsQueryable();

        userBooks = userBooks.Where(e => e.UserId == request.UserId);

        if (request.UserId != request.RequestUserId)
            userBooks = userBooks.Where(e => e.Status == UserBookItemStatus.ActivePublic);
        else
            userBooks = userBooks.Where(e => e.Status == UserBookItemStatus.ActivePublic || e.Status == UserBookItemStatus.ActivePrivate);

        if (request.Title is not null)
        {
            userBooks.Include(e => e.BookReference);
            userBooks = userBooks.Where(e => e.BookReference.Title.ToLower().Contains(request.Title.ToLower()));
        }

        userBooks = userBooks.OrderBy(e => e.CreateDate);
        var total = userBooks.Count();
        userBooks = userBooks
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        userBooks = userBooks.Include(e => e.BookReference).ThenInclude(e => e.Authors);

        return request.PaginationOptions.ToPaginatedResult(
            userBooks.ToList().Select(e => e.ToHandlerResult()).ToList(),
            total
            );
    }
}
