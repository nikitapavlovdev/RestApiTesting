using EfMobWebApiApp.Models;

namespace EfMobWebApiApp.Services.Endpoints
{
    public interface IEndpointsServices
    {
        RegisterEndpointsResponse Registrate(WebApplication application);
    }
}
