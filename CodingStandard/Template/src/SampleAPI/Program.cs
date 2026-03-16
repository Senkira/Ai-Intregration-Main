// Program.cs — .NET 10 Minimal Hosting
// Coding Standard Reference Implementation
// §6 Dependency Injection · §7 Authentication · §10 OpenAPI

using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SampleAPI.DataAccess.Connections;
using SampleAPI.DataAccess.Repositories;
using SampleAPI.DataAccess.Repositories.Interfaces;
using SampleAPI.Filters;
using SampleAPI.Middleware;
using SampleAPI.Services;
using SampleAPI.Services.Interfaces;
using SampleAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════════════
// §6.2 — DI Registration: จัดกลุ่มตามลำดับ
// ═══════════════════════════════════════════════════════

// ─── ① Singleton — Stateless, Thread-safe (§6.3) ───
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// ─── ② Scoped — Repository (1 per HTTP Request) (§6.4) ───
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// ─── ③ Scoped — Service (1 per HTTP Request) (§6.4) ───
builder.Services.AddScoped<ICustomerService, CustomerService>();

// ─── ④ Infrastructure ───

// §7.1 — JWT Bearer Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // §7.2 — Config จาก appsettings ห้าม Hard-code
        var jwtSection = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // §7.3 — Validate ทุก parameter = true
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!)),
            // §7.4 — Token หมดอายุ = ใช้ไม่ได้ทันที
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// §15.1 — FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

// §10.1 — OpenAPI (built-in .NET 10, ไม่ใช้ Swashbuckle)
builder.Services.AddOpenApi();

// §5.2 — Controllers + Filters
builder.Services.AddControllers(options =>
{
    // §8.7 — LogFilter auto log ทุก Request/Response
    options.Filters.Add(typeof(LogFilter));
    options.Filters.Add(typeof(AvailableFilter));
});

var app = builder.Build();

// ═══════════════════════════════════════════════════════
// Middleware Pipeline — ลำดับสำคัญ
// ═══════════════════════════════════════════════════════

// §16.2 — CorrelationId Middleware (ก่อน middleware อื่น)
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    // §10.1 — OpenAPI endpoint
    app.MapOpenApi();
}

// §7.5 — Middleware ต้องเรียง: Authentication → Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
