using Cinema.Core.Models;
using Cinema.DAL.DbCon;
using Cinema.DAL.Repositories.Interfaces;
using Cinema.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.DAL.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(int sessionId)
        {
            return await _dbSet.Where(b => b.SessionId == sessionId).ToListAsync();
        }
    }


}
