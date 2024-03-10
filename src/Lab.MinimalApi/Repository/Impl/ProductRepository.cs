using Lab.MinimalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace Lab.MinimalApi;

public class ProductRepository : IProductRepository
{
    private readonly EFDbContext dbContext;
    public ProductRepository(EFDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateAsync(Product entity)
    {
        await dbContext.AddAsync(entity);
    }

    public async Task<ICollection<Product>> GetAllAsync()
    {
        return await dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetAsync(int id)
    {
        return await dbContext.Products.FirstOrDefaultAsync(x => x.Id==id);
    }

    public async Task<Product?> GetAsync(string name)
    {
        return await dbContext.Products.FirstOrDefaultAsync(x =>x.Name!.Equals(name,StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task RemoveAsync(Product entity)
    {
        await Task.Run(()=>dbContext.Remove(entity));
    }

    public async Task SaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product entity)
    {
        await Task.Run(()=> dbContext.Update(entity));
    }
}
