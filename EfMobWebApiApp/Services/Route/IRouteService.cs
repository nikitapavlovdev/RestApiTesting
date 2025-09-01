using EfMobWebApiApp.Models;

namespace EfMobWebApiApp.Services.Route
{
    public interface IRouteService
    {
        UploadResponse LoadFromFile(string fromFileContent);
        SearchResponse<List<string>> SearchAdsPlatforms(string route);
    }
}