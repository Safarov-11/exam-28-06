using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public class Table
{
    [Key]
    public int Id { get; set; }
    public int Number { get; set; }
    public int Seats { get; set; }
    public bool IsReserved { get; set; } = false;


    public List<Reservation> Reservations { get; set; }
}
