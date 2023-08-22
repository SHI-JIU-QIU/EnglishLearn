

using Common;
using FileService.Infrastructure;
using FileService.Infrastructure.service;
using FileService.WebAPI.Controllers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.SetDbConfiguration<FSDbContext>();
builder.SetCommonConfiguration(new InitializerOptions{ 
    LogFilePath = "e:/temp/FileService.log",
    EventBusQueueName = "FileService.WebAPI",
});
ModuleInitializer.Initialize(builder.Services);


builder.Services.Configure<StorageSMBClientOptions>(builder.Configuration.GetSection("FileServiceSMB"));

builder.Services.Configure<StorageQiniuYunClientOptions>(builder.Configuration.GetSection("FileServiceQiniuYun"));




builder.Services.AddMediatR(mr =>
{
    mr.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
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
