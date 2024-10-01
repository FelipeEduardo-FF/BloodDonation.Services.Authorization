using BloodDonation.Services.Authorization.Api;

public class Program
{
    public async static Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);
        var app = builder.Build();

        startup.Configure(app);



        await app.RunAsync();



    }


} 