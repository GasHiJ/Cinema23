using Cinema.Core.Models;
using Cinema.DAL.DbCon;
using Cinema.DAL.Repositories.Interfaces;

namespace Cinema.DAL.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;

        public IUserRepository Users { get; } = new UserRepository(context);
        public IMovieRepository Movies { get; } = new MovieRepository(context);
        public ISessionRepository Sessions { get; } = new SessionRepository(context);
        public IHallRepository Halls { get; } = new HallRepository(context);
        public IBookingRepository Bookings { get; } = new BookingRepository(context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
