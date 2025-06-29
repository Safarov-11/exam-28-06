using System.Net;
using Domain.ApiResponse;
using Domain.DTOs.CustomerDTOs;
using Domain.Entites;
using Domain.Filters;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CustomerService(DataContext context) : ICustomerService
{
    public async Task<Response<string>> AddCustomerAsync(CreateCustomerDTO customerDTO)
    {
        var customer = new Customer
        {
            FullName = customerDTO.FullName,
            Phone = customerDTO.Phone
        };

        await context.Customers.AddAsync(customer);
        var res = await context.SaveChangesAsync();
        return res == 0
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }

    public async Task<Response<string>> DeleteCustomerAsync(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            return new Response<string>("Customer not found", HttpStatusCode.NotFound);
        }

        context.Customers.Remove(customer);
        var res = await context.SaveChangesAsync();
        return res == 0
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }

    public async Task<Response<GetCustomerDTO?>> GetCustomerAsync(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            return new Response<GetCustomerDTO?>("Customer not found", HttpStatusCode.NotFound);
        }

        var customerDTO = new GetCustomerDTO
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Phone = customer.Phone
        };

        return new Response<GetCustomerDTO?>(customerDTO);
    }

    public async Task<Response<List<GetCustomerDTO>>> GetCustomersAsync()
    {
        var Customers = await context.Customers
        .Select(c => new GetCustomerDTO
        {
            Id = c.Id,
            FullName = c.FullName,
            Phone = c.Phone
        }).ToListAsync();

        return new Response<List<GetCustomerDTO>>(Customers);

    }

    public async Task<Response<List<GetCustomerDTO>>> GetCustomersAsync(CustomerFilter filter)
    {
        var customers = context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            customers = customers.Where(c => c.FullName.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(filter.Phone))
        {
            customers = customers.Where(c => c.Phone.ToLower().Trim().Contains(filter.Phone.ToLower().Trim()));
        }

        var filterCustomers = await customers
        .Select(c => new GetCustomerDTO
        {
            Id = c.Id,
            FullName = c.FullName,
            Phone = c.Phone
        }).ToListAsync();

        return new Response<List<GetCustomerDTO>>(filterCustomers);

    }

    public async Task<Response<string>> UpdateCustomerAsync(UpdateCustomerDTO customerDTO)
    {
        var foundedCustomer = await context.Customers.FindAsync(customerDTO.Id);
        if (foundedCustomer == null)
        {
            return new Response<string>("Customer not found", HttpStatusCode.NotFound);
        }

        foundedCustomer.FullName = customerDTO.FullName;
        foundedCustomer.Phone = customerDTO.Phone;

        var res = await context.SaveChangesAsync();
        return res == 0
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }
}
