using Cinema.Core.Models;
using Cinema.DAL.DbCon;
using Cinema.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cinema.UI.DataInitializer
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovieService _movieService;
        private readonly IUnitOfWork _unitOfWork;

        public DataInitializer(IMovieService movieService, IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _context = context;
        }

        public async Task SeedMoviesAsync()
        {
            List<int> tmdbIds = new() {550, 680, 13, 155, 278, 19404, 11, 31357, 19995, 181808, 103, 37020, 19404, 14160, 27802, 635, 508,12123, 630, 640, 15224, 1998, 410, 581, 32, 43333, 43401, 548, 14291, 268, 574, 588, 903, 324, 370, 182, 451, 91, 301, 499, 524, 561, 112, 159, 2116, 27, 10674, 2751, 17821, 2831, 3351, 453, 637, 589, 789, 102, 888, 2256, 3359, 597, 713, 12123, 7890, 215, 238, 7100, 829, 604, 599, 7132, 602, 800, 540, 603, 243, 18002, 1302, 1600, 1443, 5213, 2298, 1131, 160, 388, 143, 1432, 1343, 1102, 3135, 2034, 1514, 287, 1681, 5529, 1672, 2457, 197, 4319, 4017, 3421, 4429, 1705, 3807, 4875, 1536, 5394, 6929, 4845};  

            foreach (var tmdbId in tmdbIds)
            {
                var movie = await _movieService.AddMovieByTmdbIdAsync(tmdbId);
                if (movie != null)
                {
                    movie.Id = 0;
                    _context.Movies.Add(movie);
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task SeedDataAsync()
        {
            if (!await _unitOfWork.Movies.AnyAsync())
            {
                await SeedMoviesAsync();
            }

            if (!await _unitOfWork.Users.AnyAsync())
            {
                await SeedUsersAsync();
            }

            if (!await _unitOfWork.Sessions.AnyAsync())
            {
                await SeedSessionsAsync();
            }
        }

        private async Task SeedUsersAsync()
        {
            var users = new List<User>
        {
            new User { Email = "admin@cinema.com", PasswordHash = "hashedpassword1", Role = "Admin" },
            new User { Email = "user@cinema.com", PasswordHash = "hashedpassword2", Role = "Customer" }
        };

            await _unitOfWork.Users.AddRangeAsync(users);
            await _unitOfWork.SaveAsync();
        }

        private async Task SeedSessionsAsync()
        {
            var movie = await _unitOfWork.Movies.GetByTitleAsync("Fight Club");
            if (movie == null) return;

            var halls = new List<Hall>
            {
                new() { Name = "Зал 1", Rows = 10, SeatsPerRow = 15 },
                new() { Name = "Зал 2", Rows = 8, SeatsPerRow = 12 }
            };
            _context.Halls.AddRange(halls);
            await _context.SaveChangesAsync();

            var hall = halls.First();

            var sessions = new List<Session>
            {
                new() { MovieId = movie.Id, StartTime = DateTime.UtcNow.AddDays(1), HallId = hall.Id, Price = 10.0m },
                new() { MovieId = movie.Id, StartTime = DateTime.UtcNow.AddDays(2), HallId = hall.Id, Price = 12.0m }
            };

            await _unitOfWork.Sessions.AddRangeAsync(sessions);
            await _unitOfWork.SaveAsync();

            var bookings = new List<Booking>
            {
               new() { UserId = 1, SessionId = sessions[0].Id, Row = 2, Seat = 5, Status = "active" },
               new() { UserId = 2, SessionId = sessions[0].Id, Row = 3, Seat = 8, Status = "active" }
            };

            await _unitOfWork.Bookings.AddRangeAsync(bookings);
            await _unitOfWork.SaveAsync();
        }
    }
}
