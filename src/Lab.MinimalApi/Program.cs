using Lab.MinimalApi;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//================= Step 1 : Register all Services via DI ===============================
// 1.1. Register Db Context
builder.Services.AddDbContext<EFDbContext>(option=>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
});

// 1.2. Register Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 1.3. Register EndPoint validator
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//1.4. Register security
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthRepository,AuthRepository>();

//configure swagger login
builder.Services.AddSwaggerGen(option => 
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
             "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
             "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
             "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    var securityRequirement = new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    };
    option.AddSecurityRequirement(securityRequirement);
});

//configure jwt authentication mechanism
string secretKey = builder.Configuration.GetValue<string>("ApiSettings:Secret")!;
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});

//add authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

//===========================================================


var app = builder.Build();

//=================Step 2: Register API End Point ===============================
// 2.1. Register Product end points
app.ConfigureProductEndPoints();

// 2.2 Register Auth end points
app.ConfigureAuthEndpoints();

//==============================================================================


//==================Step 3: Testing / Checking ==============================
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
