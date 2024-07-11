using MatchBook.App.Command.CreateUser;
using MatchBook.Domain.Models;
using MatchBook.Infrastructure;
using MatchBook.Infrastructure.Data;
using MatchBook.WebApi.Extensions;
using MatchBook.WebApi.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MatchBook.App.Services.Auth;
using MatchBook.App.Services;
using MatchBook.App.Services.Token;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var databaseOptions = configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>()
    ?? throw new Exception("Cannot get Database options");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(databaseOptions.ConnectionString,
   o => o.MigrationsHistoryTable(
            tableName: HistoryRepository.DefaultTableName,
            schema: "user")));

builder.Services.AddDbContext<AdminDbContext>(opt =>
    opt.UseSqlServer(databaseOptions.ConnectionString,
            o => o.MigrationsHistoryTable(
            tableName: HistoryRepository.DefaultTableName,
            schema: "admin")));


builder.Services.AddIdentity(configuration);
builder.Services.AddJWTAuthentication(configuration);

builder.Services.AddScoped<RegionRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthAdminService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
