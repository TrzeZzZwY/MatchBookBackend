namespace MatchBook.Domain.Enums;

public enum BookVisibility
{
    //Visible for all users in owner's inventory
    visible = 0,

    //Visible for owner and admins in owner's inventory
    personal = 1,

    //Visible in bookPoint
    bookPoint = 2,

    //Visible for admins in owner's inventory
    hidden = 3,

    //Visible for admins in bookPoint
    bookPointHidden = 4
}
