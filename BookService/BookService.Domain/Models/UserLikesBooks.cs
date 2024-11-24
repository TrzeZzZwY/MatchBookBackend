namespace BookService.Domain.Models;
public class UserLikesBooks
{
    public int UserId { get; set; }

    public int UserBookItemId { get; set; } 

    public UserBookItem userBookItem { get; set; }
}
