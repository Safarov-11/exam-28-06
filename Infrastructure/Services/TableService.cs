using System.Net;
using Domain.ApiResponse;
using Domain.DTOs.TableDTOs;
using Domain.Entites;
using Domain.Filters;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TableService(DataContext context) : ITableService
{
    public async Task<Response<string>> AddTableAsync(CreateTableDTO tableDTO)
    {
        var Table = new Table
        {
            Number = tableDTO.Number,
            Seats = tableDTO.Seats,
            IsReserved = tableDTO.IsReserved
        };

        await context.Tables.AddAsync(Table);
        var res = await context.SaveChangesAsync();
        return res == 0 
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }

    public async Task<Response<string>> DeleteTableAsync(int id)
    {
        var table = await context.Tables.FindAsync(id);
        if (table == null)
        {
            return new Response<string>("Table not found", HttpStatusCode.NotFound);
        }

        context.Tables.Remove(table);
        var res = await context.SaveChangesAsync();
        return res == 0
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }

    public async Task<Response<GetTableDTO?>> GetTableAsync(int id)
    {
        var table = await context.Tables.FindAsync(id);
        if (table == null)
        {
            return new Response<GetTableDTO?>("Table not found", HttpStatusCode.NotFound);
        }

        var tableDTO = new GetTableDTO
        {
            Id = table.Id,
            Number = table.Number,
            Seats = table.Seats,
            IsReserved = table.IsReserved
        };

        return new Response<GetTableDTO?>(tableDTO);
    }

    public async Task<Response<List<GetTableDTO>>> GetTablesAsync()
    {
        var tables = await context.Tables
        .Select(t => new GetTableDTO
        {
            Id = t.Id,
            Number = t.Number,
            Seats = t.Seats,
            IsReserved = t.IsReserved
        }).ToListAsync();

        return new Response<List<GetTableDTO>>(tables);

    }

    public async Task<Response<List<GetTableDTO>>> GetTablesAsync(TableFilter filter)
    {
        var Tables = context.Tables.AsQueryable();

        if (filter.Seats != null)
        {
            Tables = Tables.Where(t => t.Seats.Equals(filter.Seats));
        }

        if (filter.IsReserved != null)
        {
            Tables = Tables.Where(t => t.IsReserved.Equals(filter.IsReserved));
        }


        var filterTables = await Tables
        .Select(t => new GetTableDTO
        {
            Id = t.Id,
            Number = t.Number,
            Seats = t.Seats,
            IsReserved = t.IsReserved
        }).ToListAsync();

        return new Response<List<GetTableDTO>>(filterTables);

    }

    public async Task<Response<string>> UpdateTableAsync(UpdateTableDTO tableDTO)
    {
        var foundedTable = await context.Tables.FindAsync(tableDTO.Id);
        if (foundedTable == null)
        {
            return new Response<string>("Table not found", HttpStatusCode.NotFound);
        }

        foundedTable.Number = tableDTO.Number;
        foundedTable.Seats = tableDTO.Seats;
        foundedTable.IsReserved = tableDTO.IsReserved;

        var res = await context.SaveChangesAsync();
        return res == 0
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }
}
