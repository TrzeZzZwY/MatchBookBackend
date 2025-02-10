namespace ReportingService.Application.Clients.Dto;
public class ChangeUserStatusRequest
{
    public int UserId { get; init; }

    public AccountStatus Status { get; init; }
}
public enum AccountStatus
{
    REMOVED = 0,
    BANED = 1,
    ACTIVE = 2
}
