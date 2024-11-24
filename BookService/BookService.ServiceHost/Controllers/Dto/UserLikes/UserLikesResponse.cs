using BookService.ServiceHost.Controllers.Dto.UserItemBook;

namespace BookService.ServiceHost.Controllers.Dto.UserLikes;

public class UserLikesResponse
{
    public required int UserId { get; set; }

    public required PaginationWrapper<UserBookItemResponse> UserLikes { get; set; }
}
