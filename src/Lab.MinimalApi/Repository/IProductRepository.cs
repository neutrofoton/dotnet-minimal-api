using Lab.MinimalApi.Model;

namespace Lab.MinimalApi;

public interface IProductRepository
{
    Task<ICollection<Product>> GetAllAsync();
    Task<Product?> GetAsync(int id);
    Task<Product?> GetAsync(string name);
    Task CreateAsync(Product entity);
    Task UpdateAsync(Product entity);
    Task RemoveAsync(Product entity);
    Task SaveAsync();
}
