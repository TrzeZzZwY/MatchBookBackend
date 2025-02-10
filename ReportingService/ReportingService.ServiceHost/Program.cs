using BookService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReportingService.ServiceHost.Middleware;
using ReportingService.Application.Handlers.CreateCase;
using ReportingService.ServiceHost.Utils;
using ReportingService.Application.Clients;
using System.Net.Http.Headers;
using ReportingService.Application.Strategies;
using ReportingService.Domain.Models;
using ReportingService.Application.Strategies.RejectCase;

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
    config.RegisterServicesFromAssembly(typeof(CreateCaseCommand).Assembly);
});

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

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();
builder.Services.AddAuthorization();


var httpConfiguration = configuration.GetSection("HttpClientConfiguration").Get<HttpClientConfiguration>()
    ?? throw new Exception("Cannot get HttpClient options");

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<BookServiceClient>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = httpConfiguration.BookSeriveUrl;
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    });

builder.Services.AddHttpClient<AccountServiceClient>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = httpConfiguration.AccountSeriveUrl;
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    });

builder.Services.AddScoped<IStrategy<CaseEntity>, RejectAuthorCaseItemStrategy>();
builder.Services.AddScoped<IStrategy<CaseEntity>, RejectBookCaseItemStrategy>();
builder.Services.AddScoped<IStrategy<CaseEntity>, RejectUserItemCaseItemStrategy>();
builder.Services.AddScoped<IStrategy<CaseEntity>, RejectUserCaseItemStrategy>();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication(); 
app.UseMiddleware<JwtValidationMiddleware>();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
