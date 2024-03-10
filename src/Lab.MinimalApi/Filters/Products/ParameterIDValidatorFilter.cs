
using System.Net;
using Lab.MinimalApi.Dto;

namespace Lab.MinimalApi;

public class ParameterIDValidatorFilter : IEndpointFilter
{
    private ILogger<ParameterIDValidatorFilter> logger;
    public ParameterIDValidatorFilter(ILogger<ParameterIDValidatorFilter> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.Arguments.SingleOrDefault(x=>x?.GetType()==typeof(int)) as int?;
        if (id == null || id<=0)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            response.ErrorMessages.Add("Id should be greater than 0");
            
            logger.Log(LogLevel.Error, "ID should be greater than 0");

            return Results.BadRequest(response);
        }
        return await next(context);
    }
}
