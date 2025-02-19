using AutoMapper;
using Cinema.BLL.DTOs;
using Cinema.Core.Models;
using Cinema.DAL.DbCon;
using Cinema.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cinema.BLL.Services.Interfaces
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<SeatDto>> GetBookedSeatsAsync(int sessionId)
        {
            return await _context.Bookings
                .Where(b => b.SessionId == sessionId)
                .Select(b => new SeatDto
                {
                    Row = b.Row,
                    Seat = b.Seat,
                    UserId = b.UserId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _unitOfWork.Bookings.GetAllAsync();
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            return _mapper.Map<BookingDto>(booking);
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByUserIdAsync(int userId)
        {
            var bookings = await _unitOfWork.Bookings.GetBookingsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<BookingDto?> CreateBookingAsync(int sessionId, int userId, int row, int seat)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null)
                return null;

            var existingBooking = await _unitOfWork.Bookings
                .FindAsync(b => b.SessionId == sessionId && b.Row == row && b.Seat == seat);

            if (existingBooking.Any())
                return null;

            var booking = new Booking
            {
                UserId = userId,
                SessionId = sessionId,
                Row = row,
                Seat = seat,
                Status = "active"
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<BookingDto>(booking);
        }
    }
}
