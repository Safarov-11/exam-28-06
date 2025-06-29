using System.Net;
using Domain.ApiResponse;
using Domain.DTOs.ReservationDTOs;
using Domain.Entites;
using Domain.Filters;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReservationService(DataContext context) : IReservationService
{
    public async Task<Response<string>> AddReservationAsync(CreateReservationDTO reservationDTO)
    {
        var table = await context.Tables.FindAsync(reservationDTO.TableId);
        if (table == null)
        {
            return new Response<string>("Table dosn't exist", HttpStatusCode.NotFound);
        }

        var customer = await context.Customers.FindAsync(reservationDTO.CustomerId);
        if (customer == null)
        {
            return new Response<string>("Customer dosn't exist", HttpStatusCode.NotFound);
        }

        if (reservationDTO.FromDate > reservationDTO.ToDate)
        {
            return new Response<string>("Uncorect date formation", HttpStatusCode.BadRequest);
        }

        var hasReservation = await context.Reservations
        .AnyAsync(r => r.TableId == reservationDTO.TableId &&
        (
                (reservationDTO.FromDate >= r.FromDate && reservationDTO.FromDate < r.ToDate) ||
                (reservationDTO.ToDate > r.FromDate && reservationDTO.ToDate <= r.ToDate) ||
                (reservationDTO.FromDate <= r.FromDate && reservationDTO.ToDate >= r.ToDate)
        ));

        if (hasReservation)
        {
            return new Response<string>("Table is already reserved", HttpStatusCode.BadRequest);
        }

        var reservation = new Reservation
        {
            CustomerId = reservationDTO.CustomerId,
            TableId = reservationDTO.TableId,
            FromDate = reservationDTO.FromDate,
            ToDate = reservationDTO.ToDate
        };

        table.IsReserved = true;

        await context.Reservations.AddAsync(reservation);
        var res = await context.SaveChangesAsync();
        return res == null
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }

    public async Task<Response<string>> DeleteReservationAsync(int id)
    {
        var reservation = await context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return new Response<string>("Reservation not found", HttpStatusCode.NotFound);
        }

        var tableId = reservation.TableId;

        context.Reservations.Remove(reservation);

        var stillHasReservations = await context.Reservations
        .AnyAsync(r => r.TableId == tableId && r.Id != reservation.Id);

        var table = await context.Tables.FindAsync(tableId);
        if (table != null)
        {
            table.IsReserved = stillHasReservations;
        }

        var res = await context.SaveChangesAsync();
        return res == 0
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }

    public async Task<Response<GetReservationDTO?>> GetReservationAsync(int id)
    {
        var reservation = await context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return new Response<GetReservationDTO?>("Reservation not found", HttpStatusCode.NotFound);
        }

        var reservationDTO = new GetReservationDTO
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            TableId = reservation.TableId,
            FromDate = reservation.FromDate,
            ToDate = reservation.ToDate
        };

        return new Response<GetReservationDTO?>(reservationDTO);
    }

    public async Task<Response<List<GetReservationDTO>>> GetReservationsAsync()
    {
        var reservations = await context.Reservations
        .Select(r => new GetReservationDTO
        {
            Id = r.Id,
            CustomerId = r.CustomerId,
            TableId = r.TableId,
            FromDate = r.FromDate,
            ToDate = r.ToDate
        }).ToListAsync();

        return new Response<List<GetReservationDTO>>(reservations);

    }

    public async Task<Response<List<GetReservationDTO>>> GetReservationsAsync(ReservationFilter filter)
    {
        var reservations = context.Reservations
        .Include(r => r.Customer)
        .Include(r=>r.Table)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.CustomerName))
        {
            reservations = reservations.Where(r => r.Customer.FullName.ToLower().Trim().Contains(filter.CustomerName.ToLower().Trim()));
        }

        if (filter.TableNumber != null)
        {
            reservations = reservations.Where(r => r.Table.Number == filter.TableNumber);
        }

        if (filter.FromDate != null)
        {
            reservations = reservations.Where(r => r.FromDate >= filter.FromDate);
        }

        if (filter.ToDate != null)
        {
            reservations = reservations.Where(r => r.ToDate <= filter.ToDate);
        }

        var filterReservations = await reservations
        .Select(r => new GetReservationDTO
        {
            Id = r.Id,
            CustomerId = r.CustomerId,
            TableId = r.TableId,
            FromDate = r.FromDate,
            ToDate = r.ToDate
        }).ToListAsync();

        return new Response<List<GetReservationDTO>>(filterReservations);

    }

    public async Task<Response<string>> UpdateReservationAsync(UpdateReservationDTO reservationDTO)
    {
        var table = await context.Tables.FindAsync(reservationDTO.TableId);
        if (table == null)
        {
            return new Response<string>("Table dosn't exist", HttpStatusCode.NotFound);
        }

        var customer = await context.Customers.FindAsync(reservationDTO.CustomerId);
        if (customer == null)
        {
            return new Response<string>("Customer dosn't exist", HttpStatusCode.NotFound);
        }

        if (reservationDTO.FromDate > reservationDTO.ToDate)
        {
            return new Response<string>("Uncorect date formation", HttpStatusCode.BadRequest);
        }

        var hasReservation = await context.Reservations
        .AnyAsync(r => r.Id != reservationDTO.Id &&
        (
                (reservationDTO.FromDate >= r.FromDate && reservationDTO.FromDate < r.ToDate) ||
                (reservationDTO.ToDate > r.FromDate && reservationDTO.ToDate <= r.ToDate) ||
                (reservationDTO.FromDate <= r.FromDate && reservationDTO.ToDate >= r.ToDate)
        ));

        if (hasReservation)
        {
            return new Response<string>("Table is already reserved", HttpStatusCode.BadRequest);
        }
        
        table.IsReserved = true;

        var foundedReservation = await context.Reservations.FindAsync(reservationDTO.Id);
        if (foundedReservation == null)
        {
            return new Response<string>("Reservation not found", HttpStatusCode.NotFound);
        }

        foundedReservation.CustomerId = reservationDTO.CustomerId;
        foundedReservation.TableId = reservationDTO.TableId;
        foundedReservation.FromDate = reservationDTO.FromDate;
        foundedReservation.ToDate = reservationDTO.ToDate;

        var res = await context.SaveChangesAsync();
        return res == 0
        ? new Response<string>("Someting went wrong", HttpStatusCode.InternalServerError)
        : new Response<string>(null);
    }
}
