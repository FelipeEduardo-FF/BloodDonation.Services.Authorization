using BloodDonation.Services.Authorization.Application;
using BloodDonation.Services.Authorization.Infra;
using Shared.Infra;
using Shared.Infra.Filters;

namespace BloodDonation.Services.Authorization.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.Filters.Add(new ApiResponseFilter());
            }).Services.ConfigureOptions<ConfigureInvalidModelStateResponse>();

            services.AddEndpointsApiExplorer();

            services.AddSharedInfraModules("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });
            services.AddBloodDonationInfraModules();
            services.AddBloodDonationApplicationModules();



            


        }

        public void Configure(WebApplication app)
        {

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
                    c.RoutePrefix = "swagger";

                });
            }

            app.UseHttpsRedirection();


            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.AddGlobalErrorHandler();

            try
            {
                app.ApplyMigrations();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }


        }

    }
}
