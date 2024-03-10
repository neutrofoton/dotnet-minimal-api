using FluentValidation;
using Lab.MinimalApi.Dto.Product.Request;

namespace Lab.MinimalApi;

public class ProductUpdateValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateValidator()
    {
        RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
        RuleFor(model => model.Name).NotEmpty();
        RuleFor(model => model.Price).InclusiveBetween(1, 100);
    }
}

