namespace BookService.Application.Clients.Dto;
public class CreateCaseRequest
{
    public int UserId { get; set; }

    public CaseItemType Type { get; set; }

    public Dictionary<string, string> CaseFields { get; set; }

    public int ItemId { get; set; }
}

public enum CaseItemType
{
    AUTHOR = 1,
    BOOK = 2,
    USERITEM = 3,
    USER = 4
}
