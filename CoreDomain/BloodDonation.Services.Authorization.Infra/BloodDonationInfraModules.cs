using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using BloodDonation.Services.Authorization.Infra.Persistence;

namespace BloodDonation.Services.Authorization.Infra
{
    public static class BloodDonationInfraModules
    {
        public static IServiceCollection AddBloodDonationInfraModules(this IServiceCollection services)
        {
            services.AddDatabase();
            services.AddRepositories();
            return services;
        }       
        
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {



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




        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            return app;
        }

    }
}
