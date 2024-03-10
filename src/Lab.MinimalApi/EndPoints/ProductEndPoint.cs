using System.Net;
using Lab.MinimalApi.Dto;
using Lab.MinimalApi.Dto.Product;
using Lab.MinimalApi.Dto.Product.Request;
using Lab.MinimalApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab.MinimalApi;

public static class ProductEndPoint
{
    public static void ConfigureProductEndPoints(this WebApplication app)
    {
        app.MapGet("/api/product/",GetAll)
            .WithName("GetProducts")
            .Produces<ApiResponse>((int)HttpStatusCode.OK)
            //.RequireAuthorization()
            //.RequireAuthorization("AdminOnly") //using auth policy
            ;

        app.MapGet("/api/product/{id:int}",GetById)
            .WithName("GetProduct")
            .Produces<ApiResponse>((int)HttpStatusCode.OK)
            .AddEndpointFilter<ParameterIDValidatorFilter>();
        
        app.MapPost("/api/product", Create)
            .WithName("Create")
            .Accepts<ProductCreateRequest>("application/json")
            .Produces<ApiResponse>(201)
            .Produces(400)
            .AddEndpointFilter<RequestValidatorFilter<ProductCreateRequest>>();

        app.MapPut("/api/product", Update)
            .WithName("Update")
            .Accepts<ProductUpdateRequest>("application/json")
            .Produces<ApiResponse>(200)
            .Produces(400)
            .AddEndpointFilter<RequestValidatorFilter<ProductUpdateRequest>>();

         app.MapDelete("/api/product/{id:int}",Delete)
            .AddEndpointFilter<ParameterIDValidatorFilter>();
     
    }

    [Authorize]
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

    [Authorize]
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

    [Authorize]
    private async static Task<IResult> Create(ILogger<Product> logger, IProductRepository repository, 
             [FromBody] ProductCreateRequest productCreateRequest)
    {
        ApiResponse response = new() 
        { 
            IsSuccess = false, 
            StatusCode = HttpStatusCode.BadRequest 
        };

        if (repository.GetAsync(productCreateRequest.Name!).GetAwaiter().GetResult() != null)
        {
            response.ErrorMessages.Add("Product name already exists");
            return Results.BadRequest(response);
        }

        Product product = productCreateRequest.ToModel();


        await repository.CreateAsync(product);
        await repository.SaveAsync();

        //id will be filled automatically.
        ProductDTO productDTO = product.ToDto();

        response.Result = productDTO;
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.Created;

        return Results.Ok(response);
        //return Results.CreatedAtRoute("GetProduct",new { id=product.Id }, productDTO);
        //return Results.Created($"/api/product/{product.Id}",productDTO);
    }

    [Authorize]
    private async static Task<IResult> Update(ILogger<Product> logger, IProductRepository repository, 
             [FromBody] ProductUpdateRequest productUpdateRequest)
    {
        ApiResponse response = new() 
        { 
            IsSuccess = false, 
            StatusCode = HttpStatusCode.BadRequest 
        };

        Product product = productUpdateRequest.ToModel();
       
        await repository.UpdateAsync(product);
        await repository.SaveAsync();

        ProductDTO productDTO = product.ToDto();
        response.Result = productDTO;
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.OK;

        return Results.Ok(response);
    }

    [Authorize]
    private async static Task<IResult> Delete(ILogger<Product> logger, IProductRepository repository,  int id)
    {
       ApiResponse response = new() 
       { 
           IsSuccess = false, 
           StatusCode = HttpStatusCode.BadRequest 
       };


       Product? existingProduct = await repository.GetAsync(id);
       if (existingProduct != null)
       {
           await repository.RemoveAsync(existingProduct);
           await repository.SaveAsync();

           response.IsSuccess = true;
           response.StatusCode = HttpStatusCode.NoContent;

           return Results.Ok(response);
       }
       else
       {
           response.ErrorMessages.Add($"Product not found with id = {id}");
           return Results.BadRequest(response);
       }
    }

}
