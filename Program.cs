using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_pravotorova.Data;
using wt_lab7_7postgres_efcore_pravotorova.Models;

var builder = WebApplication.CreateBuilder(args);

// === 1. НАСТРОЙКА ПОДКЛЮЧЕНИЯ К БД ===
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var connectionString = "Host=localhost;Port=5432;Database=wt_lab7;Username=postgres;Password=123456";

// Если мы на Render, используем DATABASE_URL (Блок 26)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);
    
    // Если порт не указан (что бывает на Render), используем стандартный 5432
    int port = uri.Port > 0 ? uri.Port : 5432;

    connectionString = $"Host={uri.Host};" +
                       $"Port={port};" +
                       $"Database={uri.AbsolutePath.TrimStart('/')};" +
                       $"Username={Uri.UnescapeDataString(userInfo[0])};" +
                       $"Password={Uri.UnescapeDataString(userInfo[1])};" +
                       $"SSL Mode=Require;Trust Server Certificate=true"; // Исправлено для облака
}

// Подключаем DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
Console.WriteLine("МОЯ СТРОКА: " + connectionString);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === 2. АВТОМАТИЧЕСКОЕ ПРИМЕНЕНИЕ МИГРАЦИЙ (Блок 12) ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger UI
if (app.Environment.IsDevelopment() || true) // Включаем всегда для удобства
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// === 3. ЭНДПОИНТЫ API ===

app.MapGet("/", () => "Minimal API с PostgreSQL работает!");
app.MapGet("/api/info", () => new { App = "Hotel API", Db = "PostgreSQL" });

// GET: Список всех
app.MapGet("/api/rooms", async (AppDbContext db) =>
    await db.Rooms.ToListAsync());

// GET: Один по ID
app.MapGet("/api/rooms/{id}", async (int id, AppDbContext db) =>
{
    var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id);
    if (room == null) return Results.NotFound();
    return Results.Ok(room);
});

// GET: Поиск
app.MapGet("/api/rooms/search", async (string? search, AppDbContext db) =>
{
    var query = db.Rooms.AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(x => x.Number.ToLower().Contains(search.ToLower()) ||
                                 x.Category.ToLower().Contains(search.ToLower()));
    }
    return Results.Ok(await query.ToListAsync());
});

// POST: Создание
app.MapPost("/api/rooms", async (Room room, AppDbContext db) =>
{
    db.Rooms.Add(room);
    await db.SaveChangesAsync();
    return Results.Created($"/api/rooms/{room.Id}", room);
});

// PUT: Обновление
app.MapPut("/api/rooms/{id}", async (int id, Room updatedRoom, AppDbContext db) =>
{
    var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id);
    if (room == null) return Results.NotFound();

    room.Number = updatedRoom.Number;
    room.Category = updatedRoom.Category;
    room.Capacity = updatedRoom.Capacity;
    room.PricePerNight = updatedRoom.PricePerNight;
    room.IsAvailable = updatedRoom.IsAvailable;
    room.Description = updatedRoom.Description;

    await db.SaveChangesAsync();
    return Results.Ok(room);
});

// DELETE: Удаление
app.MapDelete("/api/rooms/{id}", async (int id, AppDbContext db) =>
{
    var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id);
    if (room == null) return Results.NotFound();

    db.Rooms.Remove(room);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Сортировка по цене
app.MapGet("/api/rooms/sorted", async (AppDbContext db) =>
    await db.Rooms.OrderBy(r => r.PricePerNight).ToListAsync());

// Фильтрация: только доступные и дороже 5000
app.MapGet("/api/rooms/expensive", async (AppDbContext db) =>
    await db.Rooms.Where(r => r.IsAvailable && r.PricePerNight > 5000).ToListAsync());

// === 4. ЗАПУСК ===
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");
