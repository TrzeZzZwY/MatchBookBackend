using BookService.Application.Clients;
using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.Exchange.GetExchanges;
public class GetExchangesHandler : IRequestHandler<GetExchangesCommand, Result<PaginatedResult<GetExchangesResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly AccountServiceClient _accountServiceClient;

    public GetExchangesHandler(DatabaseContext databaseContext, AccountServiceClient accountServiceClient)
    {
        _databaseContext = databaseContext;
        _accountServiceClient = accountServiceClient;
    }
    public async Task<Result<PaginatedResult<GetExchangesResult>, Error>> Handle(GetExchangesCommand request, CancellationToken cancellationToken)
    {
        var exchanges = _databaseContext.BookExchanges.AsQueryable();

        if (request.ReceiverUserId is not null)
            exchanges = exchanges.Where(e => e.ReceiverUserId == request.ReceiverUserId);

        if (request.InitiatorUserId is not null)
            exchanges = exchanges.Where(e => e.InitiatorUserId == request.InitiatorUserId);

        if (request.Status is not null)
            exchanges = exchanges.Where(e => e.Status == request.Status);

        var total = exchanges.Count();
        exchanges = exchanges
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        var exchangesResult = new List<GetExchangesResult>();
        foreach (var item in (exchanges.ToList()))
        {
            var initiator = await _accountServiceClient.FetchUser(item.InitiatorUserId);
            var initiatorBook = await _databaseContext.UserBookItems.Where(e => e.Id == item.InitiatorBookItemId).Include(e => e.BookReference).FirstOrDefaultAsync(cancellationToken);

            var receiver = await _accountServiceClient.FetchUser(item.ReceiverUserId);
            var receiverBook = await _databaseContext.UserBookItems.Where(e => e.Id == item.ReceiverBookItemId).Include(e => e.BookReference).FirstOrDefaultAsync(cancellationToken);

            exchangesResult.Add(
                new GetExchangesResult
                {
                    ExchangeId = item.Id,
                    Status = item.Status,
                    Initiator = new ExchangeItem
                    {
                        UserId = initiator.Value.Id,
                        UserFirstName = initiator.Value.FirstName,
                        UserLastName = initiator.Value.LastName,
                        BookTitle = initiatorBook.BookReference.Title,
                        BookImageId = initiatorBook.ItemImageId,
                        BookItemId = item.InitiatorBookItemId
                    },
                    Receiver = new ExchangeItem
                    {
                        UserId = receiver.Value.Id,
                        UserFirstName = receiver.Value.FirstName,
                        UserLastName = receiver.Value.LastName,
                        BookTitle = receiverBook.BookReference.Title,
                        BookImageId = receiverBook.ItemImageId,
                        BookItemId = item.ReceiverBookItemId
                    },
                });
        }
        return request.PaginationOptions.ToPaginatedResult(exchangesResult, total);
    }
}