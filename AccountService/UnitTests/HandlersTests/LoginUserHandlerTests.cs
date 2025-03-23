﻿using AccountService.Domain.Models;
using AccountService.Repository;
using AccountService.ServiceHost.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using AccountService.Domain.Common;
using AccountService.Application.Handlers.LoginUser;

namespace UnitTests.HandlersTests;

public class LoginUserHandlerTests
{
    private IServiceProvider _serviceProvider;
    public LoginUserHandlerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<DatabaseContext>(opt =>
        opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        serviceCollection.AddIdentity();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public async Task WhenAccountIsNotFound_ThenErrorIsReturned()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var command = new LoginUserCommand
        {
            Email = "notfound",
            Password = "123"
        };

        var handler = new LoginUserHandler(userManager);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsFailure.Should().BeTrue();
        result.Error.Description.Should().Be("Invalid email or password");
        result.Error.Reason.Should().Be(ErrorReason.BadRequest);
    }
    
    [Fact]
    public async Task WhenAccountIsFound_AndPasswordIsIncorrect_ThenIsReturned()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var account = await userManager.SetupUserAccount(roleManager, context);
        var user = account.UserAccount;

        var command = new LoginUserCommand
        {
            Email = account.Email,
            Password = "123"
        };

        var handler = new LoginUserHandler(userManager);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsFailure.Should().BeTrue();
        result.Error.Description.Should().Be("Invalid email or password");
        result.Error.Reason.Should().Be(ErrorReason.BadRequest);
    }

    [Fact]
    public async Task WhenAccountIsFound_AccountIdRemoved_ThenIsReturned()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var account = await userManager.SetupUserAccount(roleManager, context);
        var user = account.UserAccount;

        account.ChangeAccountStatus(AccountStatus.REMOVED);
        await context.SaveChangesAsync(new CancellationToken());

        var command = new LoginUserCommand
        {
            Email = account.Email,
            Password = "1qazXSW@"
        };

        var handler = new LoginUserHandler(userManager);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsFailure.Should().BeTrue();
        result.Error.Description.Should().Be("Cannot login to this account");
        result.Error.Reason.Should().Be(ErrorReason.BadRequest);
    }

    [Fact]
    public async Task WhenAccountIsFound_AccountIdBanned_ThenIsReturned()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var account = await userManager.SetupUserAccount(roleManager, context);
        var user = account.UserAccount;

        account.ChangeAccountStatus(AccountStatus.BANED);
        await context.SaveChangesAsync(new CancellationToken());

        var command = new LoginUserCommand
        {
            Email = account.Email,
            Password = "1qazXSW@"
        };

        var handler = new LoginUserHandler(userManager);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsFailure.Should().BeTrue();
        result.Error.Description.Should().Be("Cannot login to this account");
        result.Error.Reason.Should().Be(ErrorReason.BadRequest);
    }

    [Fact]
    public async Task WhenAccountIsFound_AndCredentialsAreCorrect_ThenIsReturned()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var account = await userManager.SetupUserAccount(roleManager, context);
        var user = account.UserAccount;

        var command = new LoginUserCommand
        {
            Email = account.Email,
            Password = "1qazXSW@"
        };

        var handler = new LoginUserHandler(userManager);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsSuccess.Should().BeTrue();
        result.Value.AccountId.Should().Be(account.Id);
        result.Value.UserId.Should().Be(user.Id);
    }
}
