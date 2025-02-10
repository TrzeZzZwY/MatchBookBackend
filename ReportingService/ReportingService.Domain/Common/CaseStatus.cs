using System.Text.Json.Serialization;

namespace ReportingService.Domain.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaseStatus
{
    OPEN = 1,
    INREVIEW = 2,
    APPROVED = 3,
    REJECTED = 4
}