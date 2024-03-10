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

        app
            .MapGet("/api/product/{id:int}",GetById)
            .WithName("GetProduct")
            .Produces<ApiResponse>((int)HttpStatusCode.OK)
            .AddEndpointFilter<ParameterIDValidatorFilter>()
            ;
            
    }

    private async static Task<IResult> GetAll(ILogger<Product> logger, IProductRepository repository)
    {
        logger.Log(LogLevel.Information, "Endpoint of GetAll");

        ApiResponse apiResponse=new()
        {
            Result = await repository.GetAllAsync(),
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };

        return Results.Ok(apiResponse);
    }

    private async static Task<IResult> GetById(ILogger<Product> logger, IProductRepository repository, int id)
    {
        logger.Log(LogLevel.Information, "Endpoint of GetById");

        ApiResponse apiResponse=new ()
        {
            Result = await repository.GetAsync(id),
            IsSuccess=true,
            StatusCode=HttpStatusCode.OK
        };

        return Results.Ok(apiResponse);
    }

}
