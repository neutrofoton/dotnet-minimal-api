using System.Net;
using Lab.MinimalApi.Dto;
using Lab.MinimalApi.Dto.Login.Request;
using Microsoft.AspNetCore.Mvc;

namespace Lab.MinimalApi;

public static class AuthEndpoint
{
    public static void ConfigureAuthEndpoints(this WebApplication app)
    {
                   
        app.MapPost("/api/login", Login)
            .WithName("Login")
            .Accepts<LoginRequest>("application/json")
            .Produces<ApiResponse>(200)
            .Produces(400);

        app.MapPost("/api/register", Register)
            .WithName("Register")
            .Accepts<RegisterationRequest>("application/json")
            .Produces<ApiResponse>(200)
            .Produces(400);
    }
   
    private async static Task<IResult> Register(IAuthRepository authRepository, 
        [FromBody] RegisterationRequest model)
    {
        ApiResponse response = new() 
        { 
            IsSuccess = false, 
            StatusCode = HttpStatusCode.BadRequest 
        };

        bool ifUserNameisUnique = authRepository.IsUniqueUser(model.UserName!);
        if (!ifUserNameisUnique)
        {
            response.ErrorMessages.Add("Username already exists");
            return Results.BadRequest(response);
        }

        var registerResponse = await authRepository.Register(model);
        if (registerResponse == null || string.IsNullOrEmpty(registerResponse.UserName))
        {
           
            return Results.BadRequest(response);
        }

        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.OK;
        return Results.Ok(response);
       
    }
    private async static Task<IResult> Login(IAuthRepository authRepository,
       [FromBody] LoginRequest model)
    {
        ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
        var loginResponse = await authRepository.Login(model);
        if (loginResponse == null)
        {
            response.ErrorMessages.Add("Username or password is incorrect");
            return Results.BadRequest(response);
        }
        response.Result = loginResponse;
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.OK;
        return Results.Ok(response);
    }
}
