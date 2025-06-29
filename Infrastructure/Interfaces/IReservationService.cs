using Domain.ApiResponse;
using Domain.DTOs.ReservationDTOs;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IReservationService
{
    Task<Response<string>> AddReservationAsync(CreateReservationDTO reservationDTO);
    Task<Response<string>> UpdateReservationAsync(UpdateReservationDTO reservationDTO);
    Task<Response<string>> DeleteReservationAsync(int id);
    Task<Response<GetReservationDTO?>> GetReservationAsync(int id);
    Task<Response<List<GetReservationDTO>>> GetReservationsAsync();
    Task<Response<List<GetReservationDTO>>> GetReservationsAsync(ReservationFilter filter);
}
