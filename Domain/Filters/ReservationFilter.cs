namespace Domain.Filters;

public class ReservationFilter : ValidFilter
{
    public int? TableNumber { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
