# Simple E-Commerce API

A RESTful e-commerce API with authentication, cart, orders, and payment integration.  
Autentifikasiya, səbət, sifariş və ödəniş inteqrasiyası olan RESTful e-ticarət API-si.

---

## Features / Xüsusiyyətlər

- JWT access token + refresh token authentication (single-session)  
  JWT access token + refresh token autentifikasiyası (tək sessiya)
- Role-based authorization: Admin and Customer roles  
  Rol əsaslı icazə: Admin və Customer rolları
- Admin seeding from User Secrets on startup  
  Startup-da User Secrets-dən Admin seed edilməsi
- Full CRUD for Categories and Products (Admin only for writes)  
  Kateqoriyalar və Məhsullar üçün tam CRUD (yazma yalnız Admin)
- Product filtering and pagination  
  Məhsul filtrasiyası və səhifələmə
- Cart management per user  
  İstifadəçi üzrə səbət idarəsi
- Order creation from cart with stock deduction  
  Səbətdən sifariş yaratma və stok azaltma
- Stripe payment intent integration  
  Stripe ödəniş intent inteqrasiyası
- BCrypt password hashing  
  BCrypt ilə şifrə həşlənməsi
- DataAnnotations validation on all DTOs  
  Bütün DTO-larda DataAnnotations validasiyası

---

## What I Learned / Öyrəndiklərim

- Implementing refresh token rotation with SHA-256 hashing and single-session revocation  
  SHA-256 həşləmə və tək sessiya ləğvi ilə refresh token rotasiyasının tətbiqi
- Role-based authorization with ASP.NET Core Identity policies  
  ASP.NET Core-da rol əsaslı icazə sistemi
- Integrating Stripe SDK and creating payment intents  
  Stripe SDK inteqrasiyası və payment intent yaratma
- Managing cascading delete behavior explicitly in EF Core  
  EF Core-da cascade silmə davranışının açıq idarə edilməsi
- Seeding sensitive data from User Secrets at application startup  
  Tətbiq başlanğıcında həssas məlumatların User Secrets-dən seed edilməsi
- Designing service layer with custom exceptions and controller-level error mapping  
  Xüsusi istisnalar və controller səviyyəsində xəta xəritələməsi ilə servis qatının dizaynı

---

## Tech Stack / Texnologiyalar

- **ASP.NET Core** — Web framework
- **Entity Framework Core** — ORM
- **SQL Server LocalDB** — Database / Verilənlər bazası
- **JWT (System.IdentityModel.Tokens.Jwt)** — Authentication / Autentifikasiya
- **BCrypt.Net** — Password hashing / Şifrə həşlənməsi
- **Stripe.net** — Payment integration / Ödəniş inteqrasiyası

---

## Endpoints

### Auth
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/register` | Register new user / Qeydiyyat | ❌ |
| POST | `/api/auth/login` | Login / Giriş | ❌ |
| POST | `/api/auth/refresh` | Refresh access token | ❌ |
| POST | `/api/auth/logout` | Logout / Çıxış | ✅ |

### Categories
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/categories` | List all / Hamısını gətir | ❌ |
| GET | `/api/categories/{id}` | Get by ID | ❌ |
| POST | `/api/categories` | Create / Yarat | Admin |
| PUT | `/api/categories/{id}` | Update / Yenilə | Admin |
| DELETE | `/api/categories/{id}` | Delete / Sil | Admin |

### Products
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/products` | List with filters & pagination | ❌ |
| GET | `/api/products/{id}` | Get by ID | ❌ |
| POST | `/api/products` | Create / Yarat | Admin |
| PUT | `/api/products/{id}` | Update / Yenilə | Admin |
| DELETE | `/api/products/{id}` | Delete / Sil | Admin |

### Cart
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/carts` | Get user's cart / Səbəti gətir | ✅ |
| POST | `/api/carts` | Add item to cart / Səbətə əlavə et | ✅ |
| PUT | `/api/carts/cartItems/{cartItemId}` | Update item quantity / Miqdarı yenilə | ✅ |
| DELETE | `/api/carts/carts/{cartId}` | Clear cart / Səbəti təmizlə | ✅ |
| DELETE | `/api/carts/cartItems/{cartItemId}` | Remove item / Məhsulu sil | ✅ |

### Orders
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/orders` | Create order from cart / Səbətdən sifariş yarat | ✅ |
| GET | `/api/orders` | Get user's orders / Sifarişləri gətir | ✅ |
| GET | `/api/orders/{orderId}` | Get order by ID | ✅ |
| PUT | `/api/orders/{orderId}/{status}` | Update order status / Sifarişi yenilə | Admin |

**Example Response — GET `/api/orders/{orderId}`:**
```json
{
  "orderId": "a1b2c3d4-...",
  "status": "Paid",
  "totalAmount": 149.99,
  "paymentMethod": "CreditCard",
  "createdAt": "2026-10-24T10:00:00Z",
  "items": [
    {
      "id": "e5f6...",
      "productId": "g7h8...",
      "productName": "Wireless Headphones",
      "quantity": 1,
      "unitPrice": 149.99
    }
  ]
}
```

---

## Setup / Quraşdırma

1. Clone the repository / Reponu klonla:
   ```bash
   git clone https://github.com/ismailselimzade/SimpleECommerceAPI.git
   ```
2. Set up User Secrets / User Secrets-i quraşdır:
   ```bash
   dotnet user-secrets set "Jwt:Key" "your-secret-key"
   dotnet user-secrets set "Jwt:Issuer" "your-issuer"
   dotnet user-secrets set "Jwt:Audience" "your-audience"
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
   dotnet user-secrets set "Stripe:SecretKey" "your-stripe-secret-key"
   dotnet user-secrets set "SeedAdmin:Username" "admin"
   dotnet user-secrets set "SeedAdmin:Email" "admin@example.com"
   dotnet user-secrets set "SeedAdmin:PasswordHash" "your-bcrypt-hash"
   ```
3. Apply migrations / Migrasiyaları tətbiq et:
   ```bash
   dotnet ef database update
   ```
4. Run the project / Proyekti işə sal:
   ```bash
   dotnet run
   ```
