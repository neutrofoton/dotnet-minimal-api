using System.Net;
using Lab.MinimalApi.Dto;
using Lab.MinimalApi.Model;

namespace Lab.MinimalApi;

public static class ProductEndPoint
{
    public static void ConfigureProductEndPoints(this WebApplication app)
    {
        app
            .MapGet("/api/product/",GetAll)
            .WithName("GetProducts")
            .Produces<ApiResponse>((int)HttpStatusCode.OK)
            //.RequireAuthorization("Admin");
            ;
            
    }

    private async static Task<IResult> GetAll(ILogger<Product> logger, IProductRepository repository)
    {
        logger.Log(LogLevel.Information, "Endpoint of get all product");

        ApiResponse apiResponse=new()
        {
            Result = await repository.GetAllAsync(),
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };

        return Results.Ok(apiResponse);
    }
}
