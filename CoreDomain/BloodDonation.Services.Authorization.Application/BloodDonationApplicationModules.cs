using BloodDonation.Services.Authorization.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BloodDonation.Services.Authorization.Application
{
    public static class  BloodDonationApplicationModules
    {
        public static IServiceCollection AddBloodDonationApplicationModules(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
