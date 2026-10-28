namespace wt_lab7_7postgres_efcore_pravotorova.Models;

public class Guest
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Country { get; set; } = ""; // Добавили это поле
}