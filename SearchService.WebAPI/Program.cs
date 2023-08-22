using Common;
using Microsoft.Extensions.Options;
using Nest;
using SearchService.Domain;
using SearchService.Infrastructure;
using SearchService.WebAPI.Options;
using Zack.EventBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.SetCommonConfiguration(new InitializerOptions
{
    LogFilePath = "e:/temp/SearchService.log",
    EventBusQueueName = "SearchService.WebAPI"
});

builder.Services.Configure<ElasticSearchOptions>(builder.Configuration.GetSection("ElasticSearch"));

builder.Services.AddHttpClient();
builder.Services.AddScoped<IElasticClient>(sp =>
{
    var option = sp.GetRequiredService<IOptions<ElasticSearchOptions>>();
    var settings = new ConnectionSettings(option.Value.Url);
    return new ElasticClient(settings);
});
builder.Services.AddScoped<ISearchRepository, SearchRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEventBus();
app.UseCors();//∆Ù”√Cors
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
