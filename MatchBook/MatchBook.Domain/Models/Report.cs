using MatchBook.Domain.Enums;

namespace MatchBook.Domain.Models;

public class Report
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public ApplicationUser User { get; set; }

    public ReportStatus Status { get; set; }

    public ReportType Type { get; set; }

    public string Title { get; set; }

    public string Descriptiopn { get; set; }
}
