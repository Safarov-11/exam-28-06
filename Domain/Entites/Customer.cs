using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public class Customer
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; }
    public string Phone { get; set; }


    public List<Reservation> Reservations { get; set; }
}
