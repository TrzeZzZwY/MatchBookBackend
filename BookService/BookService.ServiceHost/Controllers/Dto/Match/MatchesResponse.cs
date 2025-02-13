using BookService.Domain.Common;

namespace BookService.ServiceHost.Controllers.Dto.Match;

public class MatchesResponse
{
    public List<MatchResponse> Matches { get; set; }
}

public class MatchResponse
{
    public int UserId { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public List<MatchBookPairResponse> MatchBookPair { get; set; }
}

public class MatchBookPairResponse
{
    public BookItemResponse OfferedBook { get; set; }
    public BookItemResponse RequestedBook { get; set; }

    public int? ExchangeId { get; set; }

    public ExchangeStatus? ExchangeStatus { get; set; }

    public bool? IsMyOffer { get; set; }
}


public class BookItemResponse
{
    public int UserBookItemId { get; set; }
    public string Title { get; set; }
    public int? ImageId { get; set; }
}

