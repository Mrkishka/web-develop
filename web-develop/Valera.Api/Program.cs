using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Valera.Api.Data;
using Valera.Api.Services;
// ДОБАВЛЕНО для JWT:
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. База данных
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? "Data Source=valera.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(connectionString));

// 2. Сервисы
builder.Services.AddScoped<ValeraService>();
builder.Services.AddScoped<AuthService>(); // <-- ДОБАВЛЕНО

// 3. Контроллеры
builder.Services.AddControllers();

// 4. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 6. НАСТРОЙКА JWT АУТЕНТИФИКАЦИИ (ДОБАВЛЕНО)
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Автомиграции при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Глобальный обработчик ошибок
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            error = ex?.Message ?? "Internal server error",
            status = 500
        });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();

// ВАЖНО: Порядок middleware строго такой!
app.UseAuthentication(); // 1. Кто ты? (Проверяет токен)
app.UseAuthorization();  // 2. Что тебе можно? (Проверяет [Authorize])

app.MapControllers();

app.Run();