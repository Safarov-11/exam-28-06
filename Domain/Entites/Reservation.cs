using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public class Reservation
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int TableId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    
    public Customer Customer { get; set; }
    public Table Table { get; set; }
}
