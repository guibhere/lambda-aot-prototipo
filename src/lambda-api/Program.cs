using Services;
using NLog.Extensions.Logging;
using Amazon.Lambda.Serialization.SystemTextJson;
using lambda_api.Handlers;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Repository;
using Microsoft.EntityFrameworkCore;
using Utils.Middlewares;
using Microsoft.AspNetCore.Builder;
using DataBaseContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi, opt =>
{
    opt.Serializer = new SourceGeneratorLambdaJsonSerializer<CustomSerializer>();
});

builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IRepository, Repository.Repository>();
builder.Services.AddLogging(loggingBuilder =>
    {
        // configure Logging with NLog
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog("nlog.config");
    });

builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddDbContext<DataContext>(opt => opt.UseMySQL(GetConnectionString2()));
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseStatusCodePages(async x => await Handlers.NotFound(x.HttpContext));

Handlers._logger = app.Logger;

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");
app.MapGet("/teste", Handlers.teste);
app.MapPost("/teste", Handlers.testepost);
app.MapGet("/teste/delegate/{param}", Handlers.testedelegate);
app.MapPost("/fatura", Handlers.addfatura);
app.MapPost("/pessoas", Handlers.AddPessoa);
app.MapGet("/pessoas", Handlers.GetAllPessoas);
app.MapPut("/pessoas/{id_pessoa}", Handlers.PutPessoas);
app.MapPut("/pessoas/async/{id_pessoa}", Handlers.PutPessoasAsync);
app.MapPut("/pessoas/any/{id_pessoa}", Handlers.PutPessoasAny);
app.MapPut("/pessoas/any/async/{id_pessoa}", Handlers.PutPessoasAnyAsync);

app.Run();


static string GetConnectionString()
{
    return
        $"Server={Environment.GetEnvironmentVariable("rds_endpoint")};" +
        $"Database={Environment.GetEnvironmentVariable("rds_database")};" +
        $"Port={Environment.GetEnvironmentVariable("rds_port")};" +
        $"User={Environment.GetEnvironmentVariable("rds_user")};" +
        $"Password={Environment.GetEnvironmentVariable("rds_password")}";
}

static string GetConnectionString2()
{
    return
        $"Server=database-1.c4fkfhorltm3.sa-east-1.rds.amazonaws.com;" +
        $"Database=dbteste;" +
        $"Port=3306;" +
        $"User=guibhere;" +
        $"Password=xprQdLpOaunNWROaAu4U";
}