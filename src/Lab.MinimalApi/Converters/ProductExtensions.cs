using Lab.MinimalApi.Dto.Product;
using Lab.MinimalApi.Model;

namespace Lab.MinimalApi;

public static class ProductExtensions
{
    public static Product ToModel(this ProductDTO productDTO)
    {
        return new Product()
        {
            Id = productDTO.Id.GetValueOrDefault(0),
            Name = productDTO.Name,
            Price = productDTO.Price
        };
    }

    public static ProductDTO ToDto(this Product product)
    {
        return new ProductDTO()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
