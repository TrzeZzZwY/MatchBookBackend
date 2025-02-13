using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.Exchange.CreateExchange;
public class CreateExchangeHandler : IRequestHandler<CreateExchangeCommand, Result<CreateExchangeResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public CreateExchangeHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<CreateExchangeResult, Error>> Handle(CreateExchangeCommand request, CancellationToken cancellationToken)
    {
        var initiatorBookItem = await _databaseContext.UserBookItems.FindAsync([request.InitiatorBookItemId], cancellationToken);
        var receiverBookItem = await _databaseContext.UserBookItems.FindAsync([request.ReceiverBookItemId], cancellationToken);

        if (initiatorBookItem is null || initiatorBookItem.UserId != request.InitiatorUserId ||
            receiverBookItem is null || receiverBookItem.UserId != request.ReceiverUserId || receiverBookItem.Status != UserBookItemStatus.ActivePublic)
            return new Error("Bad Request", ErrorReason.BadRequest);

        if (initiatorBookItem.Status != UserBookItemStatus.ActivePublic)
            return new Error("You can trade only public books", ErrorReason.BadRequest);

        var exchange = new BookExchange
        {
            InitiatorBookItemId = request.InitiatorBookItemId,
            InitiatorUserId = request.InitiatorUserId,
            ReceiverBookItemId = request.ReceiverBookItemId,
            ReceiverUserId = request.ReceiverUserId,
            Status = ExchangeStatus.Pending
        };

        _databaseContext.BookExchanges.Add(exchange);

        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new CreateExchangeResult();
    }
}
