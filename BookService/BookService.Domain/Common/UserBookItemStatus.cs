using System.Text.Json.Serialization;

namespace BookService.Domain.Common;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserBookItemStatus
{
    Unspecified = 0,
    ActivePublic = 1,
    ActivePrivate = 2,
    Disabled = 3,
    Removed = 4,
    BookPoint = 5
}