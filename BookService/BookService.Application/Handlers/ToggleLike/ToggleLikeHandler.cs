using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.ToggleLike;
public class ToggleLikeHandler : IRequestHandler<ToggleLikeCommand, Result<ToggleLikeResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public ToggleLikeHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<ToggleLikeResult, Error>> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var bookItem = await _databaseContext.UserBookItems.FirstOrDefaultAsync(e => e.Id == request.UserBookItemId);
        if (bookItem == null)
            return new Error($"Cannot find bookItem for id: {request.UserBookItemId}", ErrorReason.BadRequest);

        if (bookItem.UserId == request.UserId)
            return new Error($"Cannot like your own item", ErrorReason.InvalidOperation);

        var existing = await _databaseContext.UserLikesBooks.FirstOrDefaultAsync(e => e.UserId == request.UserId && e.UserBookItemId == request.UserBookItemId, cancellationToken);

        if (existing != null)
            _databaseContext.UserLikesBooks.Remove(existing);
        else
            await _databaseContext.UserLikesBooks.AddAsync(
                new UserLikesBooks
                {
                    UserBookItemId = request.UserBookItemId,
                    UserId = request.UserId
                },
                cancellationToken);

        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new ToggleLikeResult();
    }
}
