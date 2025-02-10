using BookService.Application.Clients;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetMatches;
public class GetMatchesHandler : IRequestHandler<GetMatchesCommand, Result<GetMatchesResult, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly AccountServiceClient _accountServiceClient;

    public GetMatchesHandler(DatabaseContext databaseContext, AccountServiceClient accountServiceClient)
    {
        _databaseContext = databaseContext;
        _accountServiceClient = accountServiceClient;
    }


    public async Task<Result<GetMatchesResult, Error>> Handle(GetMatchesCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountServiceClient.FetchUser(request.UserId);
        if (user.IsFailure) return user.Error;

        // All unique books that user likes
        var userLikedBooksId = _databaseContext.UserLikesBooks
            .Include(e => e.userBookItem)
            .Where(e => e.UserId == request.UserId)
            .Select(e => e.userBookItem.BookReferenceId)
            .Distinct();

        var userBooks = _databaseContext.UserBookItems
            .Where(e => e.UserId == request.UserId && e.Status == UserBookItemStatus.ActivePublic)
            .Select(e => new { e.Id, e.BookReferenceId, e.BookReference.Title, e.ItemImageId })
            .ToList();

        // All uniqie book that user have
        var userBooksId = userBooks.Select(e => e.BookReferenceId).Distinct();

        // All uniqie users that have book that user like
        var potentialMatchedUsers = _databaseContext.UserBookItems
            .Where(e => userLikedBooksId.Contains(e.BookReferenceId) && e.Status == UserBookItemStatus.ActivePublic && e.UserId != request.UserId && e.Region == user.Value.Region)
            .Select(e => e.UserId)
            .Distinct();

        // All books that users want (joining UserBookItem)
        var potentialMatchedUsersLikes = _databaseContext.UserLikesBooks
            .Include(e => e.userBookItem)
            .Join(_databaseContext.UserBookItems,
                  like => like.UserBookItemId,
                  book => book.Id,
                  (like, book) => new { like.UserId, book.BookReferenceId, book.Id, book.BookReference.Title, book.ItemImageId })
            .Where(e => potentialMatchedUsers.Contains(e.UserId) && userBooksId.Contains(e.BookReferenceId));

        var matches = _databaseContext.UserBookItems
            .Where(e =>
                potentialMatchedUsersLikes.Select(u => u.UserId).Contains(e.UserId) &&
                userLikedBooksId.Contains(e.BookReferenceId) &&
                e.Status == UserBookItemStatus.ActivePublic)
            .Join(potentialMatchedUsersLikes,
                book => book.UserId,
                pmul => pmul.UserId,
                (book, pmul) => new
                {
                    MatchedUserId = book.UserId,
                    OfferedBook = new BookItem
                    {
                        UserBookItemId = pmul.Id,
                        Title = pmul.Title,
                        ImageId = pmul.ItemImageId
                    },
                    RequestedBook = new BookItem
                    {
                        UserBookItemId = book.Id,
                        Title = book.BookReference.Title,
                        ImageId = book.ItemImageId
                    }
                })
            .GroupBy(e => e.MatchedUserId)
            .ToDictionary(
                group => group.Key,
                group => group.Select(m => new MatchBook
                {
                    OfferedBook = m.OfferedBook,
                    RequestedBook = m.RequestedBook
                }).ToList()
            );

        var matchResults = new List<Match>();
        foreach (var match in matches)
        {
            var userResult = await _accountServiceClient.FetchUser(match.Key);
            if (userResult.IsFailure)
                return userResult.Error;

            matchResults.Add(new Match
            {
                UserId = userResult.Value.Id,
                FirstName = userResult.Value.FirstName,
                LastName = userResult.Value.LastName,
                Items = match.Value
            });
        }

        return new GetMatchesResult
        {
            Matches = matchResults
        };
    }
}
