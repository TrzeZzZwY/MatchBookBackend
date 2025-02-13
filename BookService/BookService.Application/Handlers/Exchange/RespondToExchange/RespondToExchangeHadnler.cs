using BookService.Application.Handlers.Exchange.AcceptExchange;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.Exchange.RespondToExchange;
public class RespondToExchangeHadnler : IRequestHandler<RespondToExchangeCommand, Result<RespondToExchangeResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public RespondToExchangeHadnler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<RespondToExchangeResult, Error>> Handle(RespondToExchangeCommand request, CancellationToken cancellationToken)
    {
        var exchange = await _databaseContext.BookExchanges.FindAsync([request.ExchangeId], cancellationToken);

        if (exchange is null || exchange.ReceiverUserId != request.UserId || exchange.Status != ExchangeStatus.Pending) return new Error("Bad request", ErrorReason.BadRequest);

        exchange.Status = request.Accepted ? ExchangeStatus.Accepted : ExchangeStatus.Declined;

        var initiatorBookItem = await _databaseContext.UserBookItems.FindAsync([exchange.InitiatorBookItemId], cancellationToken);
        var receiverBookItem = await _databaseContext.UserBookItems.FindAsync([exchange.ReceiverBookItemId], cancellationToken);

        if (initiatorBookItem is null || receiverBookItem is null)
            return new Error("Internal Error", ErrorReason.InternalError);

        if ( initiatorBookItem.UserId != exchange.InitiatorUserId || initiatorBookItem.Status != UserBookItemStatus.ActivePublic ||
             receiverBookItem.UserId != exchange.ReceiverUserId || receiverBookItem.Status != UserBookItemStatus.ActivePublic)
        {
            exchange.Status = ExchangeStatus.Cancelled;
            await _databaseContext.SaveChangesAsync();
            return new Error("Bad request", ErrorReason.BadRequest);
        }
        if (request.Accepted)
        {
            initiatorBookItem.UserId = exchange.ReceiverUserId;
            initiatorBookItem.UpdateStatus(UserBookItemStatus.ActivePrivate);

            receiverBookItem.UserId = exchange.InitiatorUserId;
            receiverBookItem.UpdateStatus(UserBookItemStatus.ActivePrivate);
        }   

        await _databaseContext.SaveChangesAsync();

        return new RespondToExchangeResult();
    }
}
