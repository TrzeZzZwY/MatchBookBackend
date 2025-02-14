﻿using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Domain.Models;

public class Account : IdentityUser<int>
{
    public Account(string email)
    {
        Email = email;
        UserName = email;
        CreateDate = DateTime.UtcNow;
        Status = AccountStatus.ACTIVE;
    }

    public int? UserAccountId { get; private set; }

    public UserAccount? UserAccount { get; private set; }

    public int? AdminAccountId { get; private set; }

    public AdminAccount? AdminAccount { get; private set; }

    public int RefreshTokenId { get; private set; }

    public RefreshToken RefreshToken { get; private set; }

    public DateTime CreateDate { get; private set; }

    public AccountStatus Status { get; private set; }

    public Result<Account, Error> LinkAccountToUser(UserAccount userAccount)
    {
        if (AdminAccountId is not null || UserAccountId is not null)
            return new Error("Account is already linked", ErrorReason.InvalidOperation);

        UserAccount = userAccount;

        return this;
    }

    public Result<Account, Error> LinkAccountToAdmin(AdminAccount adminAccount)
    {
        if (AdminAccountId is not null || UserAccountId is not null)
            return new Error("Account is already linked", ErrorReason.InvalidOperation);

        AdminAccount = adminAccount;

        return this;
    }

    public Result<Account, Error> ChangeAccountStatus(AccountStatus newStatus)
    {
        if (Status == AccountStatus.REMOVED) return new Error("Cannot change status of removed account", ErrorReason.InvalidOperation);

        Status = newStatus;
        return this;
    }
}
