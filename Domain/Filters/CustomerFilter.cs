namespace Domain.Filters;

public class CustomerFilter : ValidFilter
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
}
