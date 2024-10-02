using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infra.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Http;
using Shared.Infra.HttpServices;
using Shared.Infra.Configurations;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace Shared.Infra
{
    public static class InfraModule
    {

        public static IServiceCollection AddSharedInfraModules(this IServiceCollection services, string versionApi, OpenApiInfo apiInfo)
        {
            services.AddSwagger(versionApi, apiInfo);
            services.AddHttpService();
            services.AddLog();
            services.AddOpenTelemetryConfiguration();


            return services;
        }

        public static IApplicationBuilder AddGlobalErrorHandler(
        this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }

        public static IServiceCollection AddLog(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.MySQL(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Log"
                )
               .CreateLogger();

            services.AddSerilog();
            return services;
        }


        public static IServiceCollection AddSwagger(
            this IServiceCollection services, string versionApi, OpenApiInfo apiInfo
            )
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(versionApi, apiInfo);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.( Remember write Bearer before token )",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                          new[] { "Bearer" }
                    }
                });

            });

            return services;
        }

        private static IServiceCollection AddHttpService(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IHttpService, HttpService>();

            return services;
        }


        public static void AddOpenTelemetryConfiguration(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var appSettings = configuration.GetSection("JaegerConfig").Get<AppSettings>();

            services.AddOpenTelemetry()
                .ConfigureResource(r =>
                    r.AddService(appSettings?.Jaeger?.ServiceName ?? "DefaultService")
                )
                .WithTracing(tracingBuilder =>
                {
                    tracingBuilder
                        .AddSource(appSettings.Jaeger.ServiceName)  // Adiciona fontes de rastreamento
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.RecordException = true;  // Habilita gravação de exceções para requisições ASP.NET Core
                        })
                        .AddHttpClientInstrumentation(options =>
                        {
                            options.RecordException = true;  // Habilita gravação de exceções para requisições HTTP externas
                        })
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.SetDbStatementForText = true;
                            options.EnableConnectionLevelAttributes = true;
                            options.RecordException = true;  // Grava exceções geradas pelo SQL
                        })
                        .AddEntityFrameworkCoreInstrumentation(options =>
                        {
                            options.SetDbStatementForText = true;  // Captura as queries SQL geradas pelo EF Core
                        })
                        .AddJaegerExporter(p =>
                        {
                            p.AgentHost = appSettings?.Jaeger?.Host;
                            p.AgentPort = appSettings?.Jaeger?.Port ?? 0;
                        });
                });
        }

    }
}
