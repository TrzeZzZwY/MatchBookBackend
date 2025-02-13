using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.Exchange.GetExchanges;
public class GetExchangesCommand : IRequest<Result<PaginatedResult<GetExchangesResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public int? InitiatorUserId { get; set; }

    public int? ReceiverUserId { get; set; }

    public ExchangeStatus? Status { get; set; }
}

