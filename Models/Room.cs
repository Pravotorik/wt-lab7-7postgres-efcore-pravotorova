namespace wt_lab7_7postgres_efcore_pravotorova.Models;

public class Room
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public string Category { get; set; } = "";
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }

    // Вот эту строку нужно добавить!
    public string Description { get; set; } = "";

    // Добавь это поле для второй миграции
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}