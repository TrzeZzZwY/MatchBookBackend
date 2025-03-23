using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountService.Domain.Models;
using AccountService.Repository;
using Microsoft.AspNetCore.Identity;

namespace UnitTests;

public static class UnitTestsExtensions
{
    public static async Task<Account> SetupAdminAccount(this UserManager<Account> accountManager, RoleManager<AccountRole> roleManager, DatabaseContext context)
    {
        await roleManager.SetupRoles();

        var email = $"admin@{Guid.NewGuid().ToString()}.com";
        var password = "1qazXSW@";
        var firstName = "Adam";
        var lastName = "Security";

        var account = (await accountManager.FindByEmailAsync(email));
        if (account is null)
        {
            account = new Account(email);
            var createResult = await accountManager.CreateAsync(account, password);
            var addToRoleResult = await accountManager.AddToRoleAsync(account, "Admin");

            var admin = new AdminAccount
            {
                FirstName = firstName,
                LastName = lastName,
                Account = account
            };

            var userEntity = await context.AdminAccounts.AddAsync(admin, new());
            account.LinkAccountToAdmin(userEntity.Entity);

            await context.SaveChangesAsync(new());
        }
        return account;
    }

    public static async Task<Account> SetupUserAccount(this UserManager<Account> accountManager, RoleManager<AccountRole> roleManager, DatabaseContext context)
    {
        await roleManager.SetupRoles();

        var email = $"user@{Guid.NewGuid().ToString()}.com";
        var password = "1qazXSW@";
        var firstName = "John";
        var lastName = "Tester";

        var account = (await accountManager.FindByEmailAsync(email));
        if (account is null)
        {
            account = new Account(email);
            var createResult = await accountManager.CreateAsync(account, password);
            var addToRoleResult = await accountManager.AddToRoleAsync(account, "User");

            var user = new UserAccount
            {
                FirstName = firstName,
                LastName = lastName,
                Account = account,
            };

            var userEntity = await context.UserAccounts.AddAsync(user, new());
            account.LinkAccountToUser(userEntity.Entity);

            await context.SaveChangesAsync(new());
        }
        return account;
    }

    public static async Task SetupRoles(this RoleManager<AccountRole> roleManager)
    {
        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new AccountRole(role));
        }
    }
}
