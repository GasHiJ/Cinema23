using Cinema.BLL.DTOs;
using Cinema.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IMovieService
{
    Task<MovieDto?> GetOrAddMovieByTmdbIdAsync(int tmdbId);
    Task<Movie?> AddMovieByTmdbIdAsync(int tmdbId);
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto> GetMovieByIdAsync(int id);
    Task<List<Movie>> GetMoviesSortedByPopularityAsync();
    Task UpdateMovieAsync(int id, MovieDto movieDto);
    Task DeleteMovieAsync(int id);
    Task<IEnumerable<MovieDto>> GetFilteredMoviesAsync(string? genre, int? year, bool onlyWithSessions);
}