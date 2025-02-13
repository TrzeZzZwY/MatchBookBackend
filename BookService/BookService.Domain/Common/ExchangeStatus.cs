using System.Text.Json.Serialization;

namespace BookService.Domain.Common;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExchangeStatus
{
    Pending = 0,
    Accepted = 1,
    Declined = 2,
    Cancelled = 3
}
