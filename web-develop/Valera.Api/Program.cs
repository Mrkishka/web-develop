using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Valera.Api.Data;
using Valera.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. База данных
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? "Data Source=valera.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(connectionString));

// 2. Сервисы
builder.Services.AddScoped<ValeraService>();

// 3. Контроллеры
builder.Services.AddControllers();

// 4. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. CORS (Разрешаем запросы с React-фронтенда)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173", // Vite (стандартный порт)
                "http://localhost:3000"  // Create React App
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Автомиграции при старте (создаст БД и таблицу, если их нет)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Глобальный обработчик ошибок (на случай непредвиденных исключений)
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
app.MapControllers();

app.Run();