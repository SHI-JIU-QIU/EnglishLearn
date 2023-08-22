using Common;
using Listening.Admin.WebAPI;
using Listening.Admin.WebAPI.Hubs;
using Listening.Domain;
using Listening.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.SetDbConfiguration<ListeningDbContext>();
builder.SetCommonConfiguration(new InitializerOptions
{
    LogFilePath = "e:/temp/Listening.Admin.log",
    EventBusQueueName = "Listening.Admin"
});

builder.Services.AddScoped<EncodingEpisodeHelper>();
builder.Services.AddScoped<ListeningDomainService>();
builder.Services.AddScoped<IListeningRepository, ListeningRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<EpisodeEncodingStatusHub>("/Hubs/EpisodeEncodingStatusHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
