using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using SimpleECommerceAPI.Data;
using SimpleECommerceAPI.Models;
using SimpleECommerceAPI.Models.Enums;
using SimpleECommerceAPI.Services;
using Stripe;
using System.Text;
using ProductService = SimpleECommerceAPI.Services.ProductService;
using TokenService = SimpleECommerceAPI.Services.TokenService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
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

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// add Admin
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    Console.WriteLine("--> [DIAGNOSTIC] Admin seeding block reached.");

    if (await db.Database.CanConnectAsync())
    {
        Console.WriteLine("--> [DIAGNOSTIC] Database connection successful.");

        var hasAdmin = await db.Users.AnyAsync(u => u.UserRole == UserRole.Admin);
        Console.WriteLine($"--> [DIAGNOSTIC] Does admin exist? {hasAdmin}");

        if (!hasAdmin)
        {
            var username = configuration["SeedAdmin:Username"] ?? configuration["SeedAdmin__Username"];
            var email = configuration["SeedAdmin:Email"] ?? configuration["SeedAdmin__Email"];
            var passwordHash = configuration["SeedAdmin:PasswordHash"] ?? configuration["SeedAdmin__PasswordHash"];

            Console.WriteLine($"--> [DIAGNOSTIC] Username: '{username}', Email: '{email}', PasswordHash is null: {passwordHash == null}");

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(passwordHash))
            {
                var admin = new User()
                {
                    Username = username,
                    Email = email,
                    PasswordHash = passwordHash,
                    UserRole = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                };

                db.Users.Add(admin);
                await db.SaveChangesAsync();
                Console.WriteLine("--> [DIAGNOSTIC] Admin user successfully created!");
            }
            else
            {
                Console.WriteLine("--> [DIAGNOSTIC] Configuration values are missing!");
            }
        }
    }
    else
    {
        Console.WriteLine("--> [DIAGNOSTIC] Cannot connect to database.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"--> [DIAGNOSTIC ERROR]: {ex.Message}");
}


// add Stripe
var secretKey = builder.Configuration["Stripe:SecretKey"];
StripeConfiguration.ApiKey = secretKey;


app.Run();
