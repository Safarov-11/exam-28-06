namespace Domain.DTOs.TableDTOs;

public class CreateTableDTO
{
    public int Number { get; set; }
    public int Seats { get; set; }
    public bool IsReserved { get; set; } = false;
}
