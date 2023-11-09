using StoreOnline.Api.ApiHandlers;
using StoreOnline.Api.Middleware;
using FluentValidation;
using StoreOnline.Infrastructure.DataSource;
using StoreOnline.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.Elasticsearch;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(config.GetConnectionString("db"));
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<DataContext>()
    .ForwardToPrometheus();

builder.Services.AddDomainServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.Load("StoreOnline.Application"), typeof(Program).Assembly);

builder.Host.UseSerilog((_, loggerConfiguration) =>
    loggerConfiguration
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config.GetValue<string>("esUrl")!))
        {
            TypeName = null,
            ModifyConnectionSettings = cx => cx.ServerCertificateValidationCallback((_, _, _, _) => true),
            AutoRegisterTemplate = true,
            IndexFormat = "dotnet-ms-{0:yyyy-MM-dd}",
        }));

SelfLog.Enable(Console.Error);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseMiddleware<AppExceptionHandlerMiddleware>();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.UseRouting().UseEndpoints(endpoint => {
    endpoint.MapMetrics();
});


app.MapGroup("/api").MapCar();
app.MapGroup("/api").MapBuy();

app.Run();

