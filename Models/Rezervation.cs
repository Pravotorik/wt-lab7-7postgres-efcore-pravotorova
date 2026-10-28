namespace wt_lab7_7postgres_efcore_pravotorova.Models;

public class Reservation
{
    public int Id { get; set; }

    // Внешние ключи
    public int RoomId { get; set; }
    public int GuestId { get; set; }

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    // Навигационные свойства (для связи таблиц)
    public Room? Room { get; set; }
    public Guest? Guest { get; set; }
}