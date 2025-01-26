namespace AccountService.Domain.Common;

public enum ErrorReason
{
    undefined = 0,
    InternalError = 1,
    BadRequest = 2,
    InvalidOperation = 3,
    Unauthorized = 4
}
