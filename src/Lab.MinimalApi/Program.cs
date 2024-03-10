using Lab.MinimalApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//=========================================================
// 1. Register Db Context
builder.Services.AddDbContext<EFDbContext>(option=>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
});

// 2. Register Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();







//===========================================================


var app = builder.Build();

//================================================
//Testing
ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

IProductRepository productRepository = builder.Services.BuildServiceProvider().GetRequiredService<IProductRepository>();
var allProducts  = await productRepository.GetAllAsync();
logger.LogInformation($"Products count = {allProducts.Count}");

//================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
