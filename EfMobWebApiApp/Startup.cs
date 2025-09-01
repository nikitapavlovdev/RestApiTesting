using EfMobWebApiApp.Services.Endpoints;
using EfMobWebApiApp.Services.Route;

namespace EfMobWebApiApp
{
    class Startup
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args); 

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<IEndpointsServices, EndpointsServices>();
            builder.Services.AddSingleton<IRouteService, RouteService>();

            WebApplication app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IEndpointsServices endpoints = scope.ServiceProvider.GetRequiredService<IEndpointsServices>();
                endpoints.Registrate(app);
            }

            app.Run();
        }
    }
}