using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SimpleECommerceAPI.Data;
using SimpleECommerceAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    var tokenValidationParameters = new TokenValidationParameters();
    tokenValidationParameters.ValidateIssuer = true;
    tokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:Issuer"];
    tokenValidationParameters.ValidateAudience = true;
    tokenValidationParameters.ValidAudience = builder.Configuration["Jwt:Audience"];
    tokenValidationParameters.ValidateIssuerSigningKey = true;
    tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
    tokenValidationParameters.ValidateLifetime = true;

    options.TokenValidationParameters = tokenValidationParameters;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // /scalar/v1
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
