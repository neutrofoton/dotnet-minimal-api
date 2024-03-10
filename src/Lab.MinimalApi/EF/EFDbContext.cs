using Lab.MinimalApi.Model;
using Microsoft.EntityFrameworkCore;

namespace Lab.MinimalApi;

public class EFDbContext : DbContext
{
    public EFDbContext(DbContextOptions<EFDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(
            new Product()
            {
                Id = 1,
                Name = "Product 1",
                Price = 101
            },
            new Product()
            {
                Id = 2,
                Name = "Product 2",
                Price = 101
            });

        //modelBuilder.Entity<LocalUser>();
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
}