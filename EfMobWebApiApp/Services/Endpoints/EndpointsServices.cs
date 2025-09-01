using EfMobWebApiApp.Models;
using EfMobWebApiApp.Services.Route;
using Microsoft.AspNetCore.Mvc;

namespace EfMobWebApiApp.Services.Endpoints
{
    public class EndpointsServices(
        IRouteService routeService, 
        ILogger<EndpointsServices> logger) : IEndpointsServices
    {
        public RegisterEndpointsResponse Registrate(WebApplication application)
        {
            try
            {
                RouteGroupBuilder EffectiveMobileTestTask = application.MapGroup("efmob");

                EffectiveMobileTestTask.MapGet("search", ([FromQuery] string route) =>
                {
                    try
                    {
                        SearchResponse<List<string>> response = routeService.SearchAdsPlatforms(route);
                        
                        return response.Success ? Results.Ok(response) : Results.BadRequest(response);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Ошибка при выполнении метода Registrate()");

                        return Results.BadRequest(new SearchResponse<List<string>>()
                        {
                            Data = [],
                            Message = ex.Message,
                            Status = "Исключение",
                            Success = false
                        });
                    }
                });

                EffectiveMobileTestTask.MapPost("upload", async (HttpRequest request) =>
                {

                    string content = "";

                    if (request.HasFormContentType)
                    {
                        if (request.Form.Files.Count > 0)
                        {
                            IFormFile file = request.Form.Files[0];
                            using StreamReader fileReader = new(file.OpenReadStream());
                            content = await fileReader.ReadToEndAsync();
                        }
                    }
                    else if(request.ContentType == "text/plain")
                    {
                        using StreamReader rawReader = new(request.Body);
                        content = await rawReader.ReadToEndAsync();
                    }
                    else
                    {
                        string defaultFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "plat_and_routes.txt");

                        if (File.Exists(defaultFilePath))
                        {
                            content = await File.ReadAllTextAsync(defaultFilePath);
                        }
                        else
                        {
                            return Results.BadRequest(new UploadResponse()
                            {
                                Message = $"Не передан файл/текст и не найден дефолтный файл\nПереданный путь: {Path.Combine(AppContext.BaseDirectory, "Data", "plat_and_routes.txt")}",
                                Status = "Нет данных",
                                Success = false
                            });
                        }
                    }

                    if (string.IsNullOrEmpty(content))
                    {
                        return Results.BadRequest(new UploadResponse
                        {
                            Success = false,
                            Message = "Файл или текстовое содержимое пустое",
                            Status = "Ошибка параметров"
                        });
                    }
                    
                    UploadResponse response = routeService.LoadFromFile(content);

                    return response.Success ? Results.Ok(response) : Results.BadRequest(response); 
                }).DisableAntiforgery();

                return new()
                {
                    Error = "",
                    Message = "Эндпоинты успешно зарегистрированы",
                    Status = "Успех",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при выполнении метода Registrate()");

                return new()
                {
                    Error = ex.Message,
                    Message = ex.Message,
                    Status = "Ошибка при регистрации эндпоинтов",
                    Success = false
                };
            }
        }
    }
}
