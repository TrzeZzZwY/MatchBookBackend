using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.Exchange.AcceptExchange;
public class RespondToExchangeCommand : IRequest<Result<RespondToExchangeResult, Error>>
{
    public int UserId { get; set; }

    public int ExchangeId { get; set; }

    public bool Accepted { get; set; }
}