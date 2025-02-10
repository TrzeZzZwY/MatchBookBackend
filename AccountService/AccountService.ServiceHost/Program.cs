using AccountService.Application.Handlers.RegisterUser;
using AccountService.Domain.Models;
using AccountService.Repository;
using AccountService.ServiceHost.Extensions;
using AccountService.ServiceHost.Utils;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//Add DB
var configuration = builder.Configuration;
var databaseOptions = configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>()
    ?? throw new Exception("Cannot get Database options");

builder.Services.AddDbContext<DatabaseContext>(opt =>
    opt.UseSqlServer(databaseOptions.ConnectionString));

//Add CQRS
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly);
});

builder.Services.AddIdentity(configuration);
// Add services to the container.
builder.Services.AddJWTAuthentication(configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme,new string[]{}}
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

//Register services here
builder.Services.RegisterServices(configuration);

//Register Http clients
//var httpClientConfiguration = configuration.GetSection("HttpClientConfig").Get<HttpClientConfig>() 
//    ?? throw new Exception("Cannot get HttpClientConfig");

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AccountRole>>();
    var roles = new[] { "Admin", "User" };

    foreach(var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new AccountRole(role));
    }

    var accountManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();
    var email = "admin@matchbook.com";
    var password = "1qazXSW@";
    var firstName = "Adam";
    var lastName = "Security";

    if ((await accountManager.FindByEmailAsync(email)) is null)
    {
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var account = new Account(email);
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
}

app.Run();