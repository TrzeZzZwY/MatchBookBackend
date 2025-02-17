﻿using AccountService.Domain.Common;

namespace AccountService.Domain.Models;
public class UserAccount
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public Region Region { get; set; }

    public int AccountId { get; set; }

    public Account Account { get; set; }
}
