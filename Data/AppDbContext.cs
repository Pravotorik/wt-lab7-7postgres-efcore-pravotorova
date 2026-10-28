using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_pravotorova.Models;

namespace wt_lab7_7postgres_efcore_pravotorova.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Таблицы в базе данных
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
      
        // 1. Настройка связей (Relationships)

        // У одной Брони (Reservation) может быть только один Номер (Room)
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany()
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Cascade); // Если удалить номер, удалятся его брони

        // У одной Брони (Reservation) может быть только один Гость (Guest)
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Guest)
            .WithMany()
            .HasForeignKey(r => r.GuestId)
            .OnDelete(DeleteBehavior.Cascade); // Если удалить гостя, удалятся его брони


        // Начальные номера (Блок 23)
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Number = "101", Category = "Стандарт", Capacity = 2, PricePerNight = 3500, IsAvailable = true, Description = "Уютный номер с видом на сад." },
            new Room { Id = 2, Number = "102", Category = "Люкс", Capacity = 2, PricePerNight = 9000, IsAvailable = true, Description = "Просторный номер с джакузи." },
            new Room { Id = 3, Number = "201", Category = "Комфорт", Capacity = 3, PricePerNight = 5200, IsAvailable = false, Description = "Семейный номер." }
        );

        // Начальные гости (по желанию, но выглядит круто)
        modelBuilder.Entity<Guest>().HasData(
            new Guest { Id = 1, FullName = "Анна Смирнова", Email = "anna@mail.com", Phone = "+375 29 111-22-33", Country = "Беларусь" },
            new Guest { Id = 2, FullName = "Иван Петров", Email = "ivan@mail.com", Phone = "+375 29 444-55-66", Country = "Россия" }
        );
    }
}