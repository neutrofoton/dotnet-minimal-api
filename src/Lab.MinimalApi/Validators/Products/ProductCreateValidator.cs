using FluentValidation;
using Lab.MinimalApi.Dto.Product.Request;

namespace Lab.MinimalApi;


public class ProductCreateValidator : AbstractValidator<ProductCreateRequest>
{
    public ProductCreateValidator()
    {
        RuleFor(model => model.Name).NotEmpty();
        RuleFor(model => model.Price).InclusiveBetween(1, 100);
    }
}
