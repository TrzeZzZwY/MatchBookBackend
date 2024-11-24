namespace BookService.ServiceHost.Controllers.Dto.UserLikes;

public class UserLikeBookRequest
{
    public int UserId { get; set; }

    public int UserBookItemId { get; set; }
}
