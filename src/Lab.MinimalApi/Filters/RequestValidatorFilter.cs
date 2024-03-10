using FluentValidation;
using Lab.MinimalApi.Dto;
using System.Net;

namespace Lab.MinimalApi;

public class RequestValidatorFilter<T> : IEndpointFilter
{
    private IValidator<T> validator;
    public RequestValidatorFilter(IValidator<T> validator)
    {
        this.validator = validator;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
    {
        //var validator = ctx.HttpContext.RequestServices.GetService<IValidator<T>>();

        ApiResponse apiResponse=new ()
        {
            IsSuccess=false,
            StatusCode = HttpStatusCode.BadRequest
        };

        if (validator is not null)
        {
            var entity = ctx.Arguments
                .OfType<T>()
                .FirstOrDefault(x => x?.GetType() == typeof(T));

            if (entity is not null)
            {
                var results = await validator.ValidateAsync((entity));
                if (!results.IsValid)
                {
                    IEnumerable<string> errors = results.Errors.Select(x => x.ErrorMessage).ToList();
                    apiResponse.ErrorMessages.AddRange(errors);

                    return apiResponse;
                }
            }
        }

        return await next(ctx);
    }
}