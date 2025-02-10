using BookService.Domain.Common;

namespace BookService.Application.Clients.Dto;
public class GetUserResponse
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public Region Region { get; set; }
}
