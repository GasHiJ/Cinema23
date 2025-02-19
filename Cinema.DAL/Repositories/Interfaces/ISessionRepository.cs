using Cinema.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IRepository<Session>
    {
        Task<IEnumerable<Session>> GetSessionsByMovieIdWithDetailsAsync(int movieId);
        Task<IEnumerable<Session>> GetSessionsByDateWithDetailsAsync(DateTime date);
        Task<IEnumerable<Session>> GetAllWithDetailsAsync();
        Task<Session?> GetByIdWithDetailsAsync(int id);
    }


}
