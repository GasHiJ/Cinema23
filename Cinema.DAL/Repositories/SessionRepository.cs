using Cinema.Core.Models;
using Cinema.DAL.DbCon;
using Cinema.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cinema.DAL.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Session>> GetSessionsByMovieIdWithDetailsAsync(int movieId)
        {
            return await _dbSet
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Where(s => s.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsByDateWithDetailsAsync(DateTime date)
        {
            return await _dbSet
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Where(s => s.StartTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .ToListAsync();
        }

        public async Task<Session?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
