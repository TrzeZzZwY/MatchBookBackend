using BookService.Domain.Common;
using CSharpFunctionalExtensions;

namespace BookService.Domain.Models;
public class UserBookItem
{

    public UserBookItem()
    {
        Status = UserBookItemStatus.Unspecified;
    }

    public int Id { get; set; }

    public int UserId { get; set; }

    public string Description { get; set; }

    public UserBookItemStatus Status { get; private set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int BookReferenceId { get; set; }

    public int? BookPointId { get; set; }

    public Book BookReference { get; set; }

    public BookPoint? BookPoint { get; set; }

    public List<UserLikesBooks>? UserLikes { get; set; }

    public Result<UserBookItem, Error> UpdateStatus(UserBookItemStatus newStatus)
    {
        Result result = (Status, newStatus) switch
        {
            (_, UserBookItemStatus.BookPoint) => BookPointId is null ?
                Result.Failure($"Cannot set status {UserBookItemStatus.BookPoint} unless bookPoint is assigned") : Result.Success(),

            (UserBookItemStatus.Unspecified, UserBookItemStatus.ActivePublic) or
            (UserBookItemStatus.Unspecified, UserBookItemStatus.ActivePrivate) or
            (UserBookItemStatus.Unspecified, UserBookItemStatus.Removed) or

            (UserBookItemStatus.ActivePublic, UserBookItemStatus.ActivePrivate) or
            (UserBookItemStatus.ActivePublic, UserBookItemStatus.Disabled) or
            (UserBookItemStatus.ActivePublic, UserBookItemStatus.Removed) or

            (UserBookItemStatus.ActivePrivate, UserBookItemStatus.ActivePublic) or
            (UserBookItemStatus.ActivePrivate, UserBookItemStatus.Disabled) or
            (UserBookItemStatus.ActivePrivate, UserBookItemStatus.Removed) or

            (UserBookItemStatus.Disabled, UserBookItemStatus.Removed) or
            (UserBookItemStatus.Disabled, UserBookItemStatus.ActivePublic) or
            (UserBookItemStatus.Disabled, UserBookItemStatus.ActivePrivate) => Result.Success(),

            (_, _) => Result.Failure($"Cannot change status from {Status} to {newStatus}")

        };

        if (result.IsSuccess)
        {
            Status = newStatus;
            return this;
        }

        return new Error(result.Error, ErrorReason.InvalidOperation);
    }
}
