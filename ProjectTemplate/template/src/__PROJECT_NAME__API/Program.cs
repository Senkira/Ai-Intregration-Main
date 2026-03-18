using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using __PROJECT_NAME__API.DataAccess.Connections;
using __PROJECT_NAME__API.DataAccess.Repositories;
using __PROJECT_NAME__API.DataAccess.Repositories.Interfaces;
using __PROJECT_NAME__API.Filters;
using __PROJECT_NAME__API.Services;
using __PROJECT_NAME__API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ─── ① Connection Factory — Singleton: stateless, thread-safe (§6.3) ───
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// ─── ② Repository Mapping — Scoped: 1 per Request (§6.4) ───
// TODO: เพิ่ม Repository DI ที่นี่
// builder.Services.AddScoped<IXxxRepository, XxxRepository>();

// ─── ③ Service Mapping — Scoped: 1 per Request (§6.4) ───
// TODO: เพิ่ม Service DI ที่นี่
// builder.Services.AddScoped<IXxxService, XxxService>();
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
                    ?? "__PROJECT_NAME__API-Default-Secret-Key-For-Dev-Only!!")),
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
        Title = "__PROJECT_NAME__ API",
        Description = "__PROJECT_NAME__ API — Coding Standard Compliant Project"
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
// TODO: เปลี่ยน InitializeDatabase ให้ตรงกับ business domain ของคุณ
await InitializeDatabase(app);

// ─── Middleware Pipeline (§7.5) ───
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "__PROJECT_NAME__ API V1"));

app.UseRouting();
app.UseAuthentication();   // ← ต้องมาก่อน Authorization (§7.5)
app.UseAuthorization();

app.MapControllers();

app.Run();

// ─── SQLite DB Initialization ───
// TODO: แก้ CREATE TABLE ให้ตรงกับ domain ของคุณ
static async Task InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    using var conn = factory.CreateMainConnection();
    await conn.OpenAsync();

    using var cmd = conn.CreateCommand();
    cmd.CommandText = @"
        -- Auth table (ใช้ได้เลย ไม่ต้องแก้)
        CREATE TABLE IF NOT EXISTS app_user (
            user_id        INTEGER PRIMARY KEY AUTOINCREMENT,
            user_name      TEXT NOT NULL UNIQUE,
            password_hash  TEXT NOT NULL,
            role           TEXT NOT NULL DEFAULT 'User',
            is_active      INTEGER NOT NULL DEFAULT 1,
            created_date   TEXT NOT NULL,
            last_login_date TEXT
        );

        -- TODO: เพิ่ม CREATE TABLE สำหรับ business domain ของคุณ
        -- CREATE TABLE IF NOT EXISTS your_table (
        --     id   INTEGER PRIMARY KEY AUTOINCREMENT,
        --     ...
        -- );
    ";
    await cmd.ExecuteNonQueryAsync();

    // Seed admin user if not exists
    using var checkCmd = conn.CreateCommand();
    checkCmd.CommandText = "SELECT COUNT(*) FROM app_user WHERE user_name = 'admin'";
    var count = (long)(await checkCmd.ExecuteScalarAsync() ?? 0);
    if (count == 0)
    {
        using var seedCmd = conn.CreateCommand();
        var hash = __PROJECT_NAME__API.Services.AuthService.HashPassword("admin123");
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
