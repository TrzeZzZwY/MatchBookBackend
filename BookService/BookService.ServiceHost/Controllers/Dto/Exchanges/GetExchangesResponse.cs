using BookService.Domain.Common;

namespace BookService.ServiceHost.Controllers.Dto.Exchanges;

public class GetExchangesResponse
{
    public required ExchangeItemResponse Initiator { get; set; }

    public required ExchangeItemResponse Receiver { get; set; }

    public required ExchangeStatus Status { get; set; }

    public required int ExchangeId { get; set; }
}

public class ExchangeItemResponse
{
    public int UserId { get; set; }

    public string UserFirstName { get; set; }

    public string UserLastName { get; set; }

    public int BookItemId { get; set; }

    public string BookTitle { get; set; }

    public int? BookImageId { get; set; }
}
