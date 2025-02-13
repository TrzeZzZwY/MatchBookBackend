using BookService.Domain.Common;

namespace BookService.Domain.Models;
public class BookExchange
{
    public int Id { get; set; }

    public int InitiatorUserId { get; set; }

    public int InitiatorBookItemId { get; set; } 

    public int ReceiverUserId { get; set; }

    public int ReceiverBookItemId { get; set; }

    public ExchangeStatus Status { get; set; }
}
