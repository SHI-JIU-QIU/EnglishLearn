using Common;
using MediaEncoder.Infrastructure;
using Zack.JWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.SetDbConfiguration<MEDbContext>();
builder.SetCommonConfiguration(new InitializerOptions
{
    LogFilePath = "e:/temp/MediaEncoder.log",
    EventBusQueueName = "MediaEncoder.WebAPI"
});

builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));


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
