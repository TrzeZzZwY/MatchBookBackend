using BookService.Domain.Common;

namespace BookService.Application.Handlers.Exchange.GetExchanges;
public class GetExchangesResult
{
    public required ExchangeItem Initiator { get; set; }

    public required ExchangeItem Receiver { get; set; }

    public required ExchangeStatus Status { get; set; }

    public required int ExchangeId { get; set; }
}

public class ExchangeItem
{
    public int UserId { get; set; }

    public string UserFirstName { get; set; }

    public string UserLastName { get; set; }

    public int BookItemId { get; set; }

    public string BookTitle { get; set; }

    public int? BookImageId { get; set; }
}
