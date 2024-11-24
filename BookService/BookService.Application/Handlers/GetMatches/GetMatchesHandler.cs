using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetMatches;
public class GetMatchesHandler : IRequestHandler<GetMatchesCommand, Result<GetMatchesResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetMatchesHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetMatchesResult, Error>> Handle(GetMatchesCommand request, CancellationToken cancellationToken)
    {
        // All unique books that user likes
        var userLikedBooksId = _databaseContext.UserLikesBooks
            .Where(e => e.UserId == request.UserId)
            .Select(e => e.userBookItem.BookReferenceId)
            .Distinct();

        // All uniqie book that user have
        var userBooksId = _databaseContext.UserBookItems
            .Where(e => e.UserId == request.UserId && e.Status == UserBookItemStatus.ActivePublic)
            .Select(e => e.BookReferenceId)
            .Distinct();

        // All uniqie users that have book that user like
        var potentialMatchedUsers = _databaseContext.UserBookItems
            .Where(e => userLikedBooksId.Contains(e.BookReferenceId) && e.Status == UserBookItemStatus.ActivePublic)
            .Select(e => e.UserId)
            .Distinct();

        // All books that users wants
        var potentialMatchedUsersLikes = _databaseContext.UserLikesBooks.Where(e => potentialMatchedUsers.Contains(e.UserId));

        // matches
        var matches = potentialMatchedUsersLikes.Where(e => userBooksId.Contains(e.UserBookItemId)).Select(e => e.UserId).Distinct();

        return new GetMatchesResult
        {
            MatchedUsersIds = matches.ToList()
        };
    }
}
