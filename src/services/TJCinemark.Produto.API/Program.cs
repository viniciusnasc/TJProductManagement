using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceStack;
using TJ.ProductManagement.API.Configuration;
using TJ.ProductManagement.Data.MongoConfiguration;
using TJ.ProductManagement.Domain.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDependencyInjectionConfiguration();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.Configure<AmazonSQSSettings>(builder.Configuration.GetSection("AmazonSQSSettings"));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.Configure<QueueUrl>(builder.Configuration.GetSection("QueueUrl"));

var app = builder.Build();

app.UseSwaggerConfiguration(app.Environment);
app.UseApiConfiguration();
app.UseAuthorization();
app.MapControllers();
app.Run();