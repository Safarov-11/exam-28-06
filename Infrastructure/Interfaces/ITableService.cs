using Domain.ApiResponse;
using Domain.DTOs.TableDTOs;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface ITableService
{
    Task<Response<string>> AddTableAsync(CreateTableDTO tableDTO);
    Task<Response<string>> UpdateTableAsync(UpdateTableDTO tableDTO);
    Task<Response<string>> DeleteTableAsync(int id);
    Task<Response<GetTableDTO?>> GetTableAsync(int id);
    Task<Response<List<GetTableDTO>>> GetTablesAsync();
    Task<Response<List<GetTableDTO>>> GetTablesAsync(TableFilter filter);
}
