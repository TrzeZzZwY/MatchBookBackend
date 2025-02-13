using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.Exchange.CreateExchange;
public class CreateExchangeCommand : IRequest<Result<CreateExchangeResult, Error>>
{
    public int InitiatorUserId { get; set; }

    public int InitiatorBookItemId { get; set; }

    public int ReceiverUserId { get; set; }

    public int ReceiverBookItemId { get; set; }
}