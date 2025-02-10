using System.Text.Json.Serialization;

namespace AccountService.Domain.Common;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountStatus
{
    REMOVED = 0,
    BANED = 1,
    ACTIVE = 2
}
