using Cinema.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<SeatDto>> GetBookedSeatsAsync(int sessionId);
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto?> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId);
        Task<BookingDto?> CreateBookingAsync(int sessionId, int userId, int row, int seat);
    }

}
