namespace Domain.Filters;

public class TableFilter : ValidFilter
{
    public int? Seats { get; set; }
    public bool? IsReserved { get; set; }
}
