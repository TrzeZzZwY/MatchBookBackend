using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetFeedHandler : IRequestHandler<GetFeedCommand, Result<PaginatedResult<GetUserBookResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetFeedHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetUserBookResult>, Error>> Handle(GetFeedCommand request, CancellationToken cancellationToken)
    {
        var userBooks = _databaseContext.UserBookItems.AsQueryable();

        userBooks = userBooks.Where(e => e.UserId != request.RequestUserId);

        userBooks = userBooks.Where(e => e.Region == request.Region);

        userBooks = userBooks.Where(e => e.Status == UserBookItemStatus.ActivePublic);

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
