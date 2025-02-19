using Cinema.Core.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cinema.DAL.Repositories.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<Movie?> FirstOrDefaultAsync(Func<Movie, bool> predicate);
        Task<bool> AnyAsync(Expression<Func<Movie, bool>> predicate);
        Task<Movie?> GetByIdWithGenreAsync(int id);
        Task<Movie?> GetByTitleAsync(string title);
        Task<Movie?> GetByTmdbIdAsync(int tmdbId);
    }
}

