using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using Serilog;
using Zack.EventBus;
using Zack.JWT;
using Microsoft.AspNetCore.HttpOverrides;
using StackExchange.Redis;

namespace Common
{
    public static class WebAppBuilderExtensions
    {

        public static void SetDbConfiguration<TContext>(this WebApplicationBuilder builder) where TContext : DbContext
        {
            string DbConnString = Environment.GetEnvironmentVariable("DbConnString", EnvironmentVariableTarget.User)!;
            builder.Services.AddDbContext<TContext>(opt => opt.UseSqlServer(DbConnString));

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<UnitOfWorkFilter>();
            });
        }

        public static void SetCommonConfiguration(this WebApplicationBuilder builder, InitializerOptions initOptions)
        {
            var webBuilder = builder.Host;
            webBuilder.ConfigureAppConfiguration((hostCtx, configBuilder) => {
                string connStr = Environment.GetEnvironmentVariable("DbConnString", EnvironmentVariableTarget.User)!;
                configBuilder.AddDbConfiguration(() => new SqlConnection(connStr), reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(2));
            });

            builder.Services.AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddLogging(builder =>
            {
                Log.Logger = new LoggerConfiguration()
                   // .MinimumLevel.Information().Enrich.FromLogContext()
                   .WriteTo.Console()
                   .WriteTo.File(initOptions.LogFilePath)
                   .CreateLogger();
                builder.AddSerilog();
            });

            builder.Services.AddEventBus(initOptions.EventBusQueueName,Assembly.GetCallingAssembly());

            builder.Services.AddCors(options =>
            {
                var corsOpt = builder.Configuration.GetSection("Cors").Get<CorsSettings>();
                string[] urls = corsOpt.Origins;
                options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();
            JWTOptions jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
            builder.Services.AddJWTAuthentication(jwtOpt);

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetCallingAssembly()));

            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<IntegrationEventRabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));

            builder.Services.AddEventBus(initOptions.EventBusQueueName, Assembly.GetCallingAssembly());

            //Redis的配置
            string redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnStr");
            IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
            builder.Services.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }


    }
}
