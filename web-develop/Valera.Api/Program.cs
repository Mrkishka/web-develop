using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Valera.Api.Data;
using Valera.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. БД
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? "Data Source=valera.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(connectionString));

// 2. Сервис
builder.Services.AddScoped<ValeraService>();

// 3. Контроллеры
builder.Services.AddControllers();

// 4. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Автомиграции при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Глобальный обработчик исключений:
// InvalidOperationException -> 400 Bad Request с текстом
// остальное                 -> 500 Internal Server Error
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        if (ex is InvalidOperationException ioe)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = ioe.Message,
                status = 400
            });
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Internal server error",
                status = 500
            });
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();