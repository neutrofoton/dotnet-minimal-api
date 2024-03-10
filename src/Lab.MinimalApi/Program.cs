using Lab.MinimalApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//================= Step 1 : Register all Services via DI ===============================
// 1. Register Db Context
builder.Services.AddDbContext<EFDbContext>(option=>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
});

// 2. Register Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 3. 

//===========================================================


var app = builder.Build();

//=================Step 2: Register API End Point ===============================
// 1. Register Product End Points
app.ConfigureProductEndPoints();

//==============================================================================


//================================================
//Testing
var scope = app.Services.CreateScope();
IServiceProvider serviceProvider = scope.ServiceProvider;

IProductRepository? productRepository  = serviceProvider.GetRequiredService<IProductRepository>();
var allProducts  = await productRepository!.GetAllAsync();
app.Logger.LogInformation($"Products count at Program = {allProducts.Count}");

//================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
