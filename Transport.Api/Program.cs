using MediatR;
using Serilog;
using Transport.Api.Exceptions;
using Transport.Application.Contract.Repositories;
using Transport.Application.Contract.Services;
using Transport.Application.Services;
using Transport.Infrastructure;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/TransportApi.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog();

var applicationContractAssembly = AppDomain.CurrentDomain.Load("Transport.Application.Contract");
if (applicationContractAssembly != null)
    builder.Services.AddMediatR(applicationContractAssembly);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IGitHubUserRepository, GitHubUserRepository>();

builder.Services.AddTransient<IGitHubUserService, GitHubUserService>();

builder.Services.AddHttpClient(nameof(GitHubUserRepository));

builder.Services.AddHttpClient<IGitHubUserRepository, GitHubUserRepository>((provider, client) =>
{
    client.BaseAddress = new Uri(GetGitHubBaseUrl());
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "transportApi/1.0");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


string GetGitHubBaseUrl()
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.Development.json")
        .Build();

    var baseUrl = configuration?["GitHubBaseUrl"];

    return !string.IsNullOrEmpty(baseUrl) ? baseUrl : string.Empty;
}