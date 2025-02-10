using System.Text.Json.Serialization;

namespace ReportingService.Domain.Common;
[JsonConverter(typeof(JsonStringEnumConverter))]

public enum ReportType
{
    AutoReport = 1,
    UserReport = 2
}
