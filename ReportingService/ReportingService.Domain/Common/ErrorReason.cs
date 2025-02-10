namespace ReportingService.Domain.Common;

public enum ErrorReason
{
    Undefined = 0,
    InternalError = 1,
    BadRequest = 2,
    InvalidOperation = 3,
    Unauthorized = 4,
    NotFound = 5,
}
