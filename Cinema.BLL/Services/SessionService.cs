using Cinema.BLL.DTOs;
using Cinema.BLL.Services.Interfaces;
using Cinema.Core.Models;
using Cinema.DAL.Repositories;
using Cinema.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cinema.BLL.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingService _bookingService;

        public SessionService(IUnitOfWork unitOfWork, IBookingService bookingService)
        {
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;
        }

        public async Task<IEnumerable<SessionDto>> GetAllSessionsAsync()
        {
            var sessions = await _unitOfWork.Sessions.GetAllWithDetailsAsync();
            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                MovieId = s.Movie.Id,
                MovieTitle = s.Movie.Title,
                StartTime = s.StartTime,
                HallName = s.Hall.Name,
                Price = s.Price,
                Rows = s.Hall.Rows,
                SeatsPerRow = s.Hall.SeatsPerRow,
                BookedSeats = s.Bookings
            .Select(b => new SeatDto { Row = b.Row, Seat = b.Seat })
            .ToList()
            });
        }

        public async Task CreateSessionAsync(CreateSessionDto sessionDto)
        {
            var session = new Session
            {
                MovieId = sessionDto.MovieId,
                HallId = sessionDto.HallId,
                StartTime = sessionDto.StartTime,
                Price = sessionDto.Price
            };

            await _unitOfWork.Sessions.AddAsync(session);
            await _unitOfWork.SaveAsync();
        }

        public async Task<SessionDto> GetSessionByIdAsync(int sessionId, int userId)
        {
            var session = await _unitOfWork.Sessions.GetByIdWithDetailsAsync(sessionId);

            if (session == null)
            {
                return null;
            }

            var bookedSeats = await _bookingService.GetBookedSeatsAsync(sessionId);

            foreach (var seat in bookedSeats)
            {
                seat.IsCurrentUserBooking = seat.UserId == userId;
            }

            return new SessionDto
            {
                Id = session.Id,
                MovieTitle = session.Movie?.Title ?? "Невідомо",
                HallName = session.Hall?.Name ?? "Невідомий зал",
                StartTime = session.StartTime,
                Price = session.Price,
                Rows = session.Hall?.Rows ?? 0,
                SeatsPerRow = session.Hall?.SeatsPerRow ?? 0,
                BookedSeats = bookedSeats,
                CurrentUserId = userId
            };
        }

        public async Task<IEnumerable<SessionDto>> GetSessionsByMovieIdAsync(int movieId)
        {
            var sessions = await _unitOfWork.Sessions.GetSessionsByMovieIdWithDetailsAsync(movieId);
            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                MovieId = s.Movie.Id,
                MovieTitle = s.Movie.Title,
                StartTime = s.StartTime,
                HallName = s.Hall.Name,
                Price = s.Price,
                Rows = s.Hall.Rows,
                SeatsPerRow = s.Hall.SeatsPerRow,
                BookedSeats = s.Bookings
            .Select(b => new SeatDto { Row = b.Row, Seat = b.Seat })
            .ToList()
            });
        }

        public async Task<IEnumerable<SessionDto>> GetSessionsByDateAsync(DateTime date)
        {
            var sessions = await _unitOfWork.Sessions.GetSessionsByDateWithDetailsAsync(date);
            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                MovieId = s.Movie.Id,
                MovieTitle = s.Movie.Title,
                StartTime = s.StartTime,
                HallName = s.Hall.Name,
                Price = s.Price,
                Rows = s.Hall.Rows,
                SeatsPerRow = s.Hall.SeatsPerRow,
                BookedSeats = s.Bookings
            .Select(b => new SeatDto { Row = b.Row, Seat = b.Seat })
            .ToList()
            });
        }

        public async Task<SessionDto> CreateSessionAsync(SessionDto sessionDto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(sessionDto.MovieId);
            var hall = await _unitOfWork.Halls.GetByNameAsync(sessionDto.HallName);

            if (movie == null || hall == null)
                throw new Exception("Invalid Movie or Hall");

            var session = new Session
            {
                MovieId = movie.Id,
                StartTime = sessionDto.StartTime,
                HallId = hall.Id,
                Price = sessionDto.Price
            };

            await _unitOfWork.Sessions.AddAsync(session);
            await _unitOfWork.SaveAsync();

            sessionDto.Id = session.Id;
            return sessionDto;
        }


        public async Task<bool> DeleteSessionAsync(int id)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (session == null) return false;

            _unitOfWork.Sessions.Remove(session);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
