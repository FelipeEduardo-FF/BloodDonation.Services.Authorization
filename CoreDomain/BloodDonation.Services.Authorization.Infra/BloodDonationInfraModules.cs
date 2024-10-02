using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using BloodDonation.Services.Authorization.Infra.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BloodDonation.Services.Authorization.Domain.Entities;
using BloodDonation.Services.Authorization.Domain.Persistence.Repositories;
using BloodDonation.Services.Authorization.Infra.Persistence.Repositories;

namespace BloodDonation.Services.Authorization.Infra
{
    public static class BloodDonationInfraModules
    {
        public static IServiceCollection AddBloodDonationInfraModules(this IServiceCollection services)
        {
            services.AddDatabase();
            services.AddAuthConfig();
            services.AddRepositories();
            return services;
        }       
        
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IAuthRepository, AuthRepository>();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var _configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(_configuration.GetConnectionString("DefaultConnection"),
                            ServerVersion.AutoDetect(_configuration.GetConnectionString("DefaultConnection"))));

            return services;
        }

        private static IServiceCollection AddAuthConfig(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var _configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }



        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            return app;
        }

    }
}
