using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartBankStatementAPI.DataAccess.Connections;
using SmartBankStatementAPI.DataAccess.Repositories;
using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.Filters;
using SmartBankStatementAPI.Services;
using SmartBankStatementAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ─── ① Connection Factory — Singleton: stateless, thread-safe (§6.3) ───
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// ─── ② Repository Mapping — Scoped: 1 per Request (§6.4) ───
builder.Services.AddScoped<IStatementRepository, StatementRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ─── ③ Service Mapping — Scoped: 1 per Request (§6.4) ───
builder.Services.AddScoped<IStatementService, StatementService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ─── ④ Infrastructure ───
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(LogFilter));
    options.Filters.Add(typeof(AvailableFilter));
});

// ─── JWT Authentication (§7) ───
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                    ?? "SmartBankStatementAPI-Default-Secret-Key-For-Dev-Only-2025!!")),
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ─── Swagger with JWT (§10) ───
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SmartBankStatement API",
        Description = "Mini Banking eStatement API — Coding Standard Reference Project"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by space and JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// ─── Initialize SQLite database (development only) ───
await InitializeDatabase(app);

// ─── Middleware Pipeline (§7.5) ───
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartBankStatement API V1"));

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// ─── SQLite DB Initialization ───
static async Task InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    using var conn = factory.CreateMainConnection();
    await conn.OpenAsync();

    using var cmd = conn.CreateCommand();
    cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS customer (
            customer_id   TEXT PRIMARY KEY,
            customer_name TEXT NOT NULL,
            email         TEXT,
            phone_number  TEXT,
            status        TEXT NOT NULL DEFAULT 'ACTIVE',
            created_date  TEXT NOT NULL,
            updated_date  TEXT
        );

        CREATE TABLE IF NOT EXISTS statement (
            statement_id  INTEGER PRIMARY KEY AUTOINCREMENT,
            contract_no   TEXT NOT NULL,
            cut_off_date  TEXT NOT NULL,
            as_of_date    TEXT NOT NULL,
            file_path     TEXT NOT NULL,
            status        TEXT NOT NULL DEFAULT 'ACTIVE',
            created_date  TEXT NOT NULL,
            updated_date  TEXT
        );

        CREATE TABLE IF NOT EXISTS app_user (
            user_id        INTEGER PRIMARY KEY AUTOINCREMENT,
            user_name      TEXT NOT NULL UNIQUE,
            password_hash  TEXT NOT NULL,
            role           TEXT NOT NULL DEFAULT 'User',
            is_active      INTEGER NOT NULL DEFAULT 1,
            created_date   TEXT NOT NULL,
            last_login_date TEXT
        );

        CREATE INDEX IF NOT EXISTS idx_statement_contract ON statement(contract_no);
        CREATE INDEX IF NOT EXISTS idx_customer_status ON customer(status);
    ";
    await cmd.ExecuteNonQueryAsync();

    // Seed admin user if not exists
    using var checkCmd = conn.CreateCommand();
    checkCmd.CommandText = "SELECT COUNT(*) FROM app_user WHERE user_name = 'admin'";
    var count = (long)(await checkCmd.ExecuteScalarAsync() ?? 0);
    if (count == 0)
    {
        using var seedCmd = conn.CreateCommand();
        var hash = SmartBankStatementAPI.Services.AuthService.HashPassword("admin123");
        seedCmd.CommandText = $@"
            INSERT INTO app_user (user_name, password_hash, role, is_active, created_date)
            VALUES ('admin', '{hash}', 'Admin', 1, '{DateTime.UtcNow:O}')";
        await seedCmd.ExecuteNonQueryAsync();
    }
}

/// <summary>
/// Partial class for integration test access
/// </summary>
public partial class Program { }
