using System.Text.Json.Serialization;

namespace ReportingService.Domain.Common;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaseItemType
{
    AUTHOR = 1,
    BOOK = 2,
    USERITEM = 3,
    USER = 4
}