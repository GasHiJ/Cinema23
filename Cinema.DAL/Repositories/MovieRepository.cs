using Cinema.Core.Models;
using Cinema.DAL.DbCon;
using Cinema.DAL.Repositories;
using Cinema.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Movie?> FirstOrDefaultAsync(Func<Movie, bool> predicate)
    {
        return await _context.Movies.FirstOrDefaultAsync(m => predicate(m));
    }


    public async Task<Movie?> GetByTitleAsync(string title)
    {
        return await _context.Movies
            .FirstOrDefaultAsync(m => m.Title == title);
    }


    public async Task<Movie?> GetByIdWithGenreAsync(int id)
    {
        return await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> AnyAsync(Expression<Func<Movie, bool>> predicate)
    {
        return await _context.Movies.AnyAsync(predicate);
    }

    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
    }

    public async Task<Movie?> GetByTmdbIdAsync(int tmdbId)
    {
        return await _context.Movies.FirstOrDefaultAsync(m => m.TmdbId == tmdbId);
    }
}

