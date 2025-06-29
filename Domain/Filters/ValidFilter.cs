namespace Domain.Filters;

public class ValidFilter
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }

    public ValidFilter()
    {

    }
    public ValidFilter(int pageSize, int pageNumber)
    {
        PageSize = pageSize < 1 ? 100 : pageSize;
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
    }
}
