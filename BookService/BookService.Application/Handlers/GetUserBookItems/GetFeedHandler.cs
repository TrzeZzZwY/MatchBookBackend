using BookService.Application.Clients;
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
    private readonly AccountServiceClient _accountServiceClient;

    public GetFeedHandler(DatabaseContext databaseContext, AccountServiceClient accountServiceClient)
    {
        _databaseContext = databaseContext;
        _accountServiceClient = accountServiceClient;
    }

    public async Task<Result<PaginatedResult<GetUserBookResult>, Error>> Handle(GetFeedCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountServiceClient.FetchUser(request.UserId);
        if (user.IsFailure) return user.Error;

        var userlikes = _databaseContext.UserLikesBooks.Where(e => e.UserId == request.UserId).Select(e => e.UserBookItemId);

        var userBooks = _databaseContext.UserBookItems.AsQueryable();

        // Not user books
        userBooks = userBooks.Where(e => e.UserId != request.UserId);

        // In user region
        userBooks = userBooks.Where(e => e.Region == user.Value.Region);

        userBooks = userBooks.Where(e => e.Status == UserBookItemStatus.ActivePublic);

        userBooks = userBooks.Where(e => !userlikes.Contains(e.Id));

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
