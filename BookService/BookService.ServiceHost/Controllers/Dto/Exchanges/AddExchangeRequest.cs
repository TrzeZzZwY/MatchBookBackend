namespace BookService.ServiceHost.Controllers.Dto.Exchanges;

public class AddExchangeRequest
{
    public int InitiatorBookItemId { get; set; }

    public int ReceiverUserId { get; set; }

    public int ReceiverBookItemId { get; set; }
}
