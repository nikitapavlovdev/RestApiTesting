using EfMobWebApiApp.Models;

namespace EfMobWebApiApp.Services.Route
{
    public class RouteService(ILogger<RouteService> logger) : IRouteService
    {
        private readonly Dictionary<string, List<string>> routesDict = [];
        public UploadResponse LoadFromFile(string fromFileContent)
        {
            try
            {
                routesDict.Clear();

                List<string> Errors = [];

                foreach (string str in fromFileContent.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] parts = str.Split(":", 2);

                    if (parts.Length != 2)
                    {
                        Errors.Add($"Ошибка в строке {str}.\nОжидаемый формат: [имя_площадки]:[route1],[route2],...");
                        continue;
                    }

                    string platform = parts[0].Trim();
                    string[] routes = parts[1].Split(",", StringSplitOptions.RemoveEmptyEntries);

                    foreach (string route in routes)
                    {
                        string endRoute = route.Trim();

                        if (!routesDict.TryGetValue(endRoute, out var value))
                        {
                            value = [];
                            routesDict[endRoute] = value;
                        }

                        value.Add(platform);
                    }
                }

                if (Errors.Count > 0)
                {
                    return new()
                    {
                        Message = string.Join("\n", Errors),
                        Status = "Выполнено с ошибками",
                        Success = false
                    };
                }

                return new()
                {
                    Message = "Словарь успешно заполнен",
                    Status = "Выполнено",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при выполнении метода LoadFromFile()");

                return new()
                {
                    Message = ex.Message,
                    Status = "Ошибка выполнения",
                    Success = false
                };
            }
        }
        public SearchResponse<List<string>> SearchAdsPlatforms(string route)
        {
            try
            {
                if (string.IsNullOrEmpty(route))
                {
                    return new()
                    {
                        Data = [],
                        Message = "Входной параметр route является пустым или null",
                        Status = "Ошибка параметров",
                        Success = false
                    };
                }

                HashSet<string> platforms = [];

                string[] parts = route.Split("/", StringSplitOptions.RemoveEmptyEntries);

                for (int i = parts.Length; i >= 1; i--)
                {
                    string prefix = "/" + string.Join("/", parts.Take(i));

                    if (routesDict.TryGetValue(prefix, out var list))
                    {
                        platforms.UnionWith(list);
                    }
                }

                if(platforms.Count == 0)
                {
                    return new()
                    {
                        Data = [],
                        Message = "Для указанного маршрута площадки не найдены",
                        Status = "Не найдено",
                        Success = false
                    };
                }

                return new()
                {
                    Data = [.. platforms],
                    Message = "Поиск выполнен успешно",
                    Status = "Успех",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при выполнении метода SearchAdsPlatforms()");

                return new()
                {
                    Data = [],
                    Message = ex.Message,
                    Status = "Ошибка выполнения",
                    Success = false
                };
            }
        }
    }
}
