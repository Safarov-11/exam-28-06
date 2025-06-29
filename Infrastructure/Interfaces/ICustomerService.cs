using Domain.ApiResponse;
using Domain.DTOs.CustomerDTOs;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface ICustomerService
{
    Task<Response<string>> AddCustomerAsync(CreateCustomerDTO customerDTO);
    Task<Response<string>> UpdateCustomerAsync(UpdateCustomerDTO customerDTO);
    Task<Response<string>> DeleteCustomerAsync(int id);
    Task<Response<GetCustomerDTO?>> GetCustomerAsync(int id);
    Task<Response<List<GetCustomerDTO>>> GetCustomersAsync();
    Task<Response<List<GetCustomerDTO>>> GetCustomersAsync(CustomerFilter filter);
}
