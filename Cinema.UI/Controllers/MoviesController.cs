using Cinema.BLL.Services.Interfaces;
using Cinema.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cinema.DAL.DbCon;
using Cinema.Core.Models;
using Microsoft.EntityFrameworkCore;
using Cinema.BLL.Services;
using System.Net.Http;
using Cinema.BLL.DTOs;

namespace Cinema.UI.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = new MovieDto
            {
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                Genre = movie.Genre,
                ReleaseYear = movie.ReleaseYear,
                Rating = movie.Rating,
                Actors = movie.Actors,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl
            };

            return View(movieDto);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? genre, bool? hasSessions, string? movieTitle)
        {
            var movies = await _movieService.GetAllMoviesAsync();

            var uniqueGenres = movies
                .SelectMany(m => m.Genre.Split(", ").Select(g => g.Trim()))
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            if (!string.IsNullOrEmpty(movieTitle))
            {
                movies = movies
                    .Where(m => m.Title.Contains(movieTitle, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(genre))
            {
                movies = movies
                    .Where(m => m.Genre.Split(", ").Select(g => g.Trim().ToLower()).Contains(genre.ToLower()))
                    .ToList();
            }

            if (hasSessions == true)
            {
                movies = movies.Where(m => m.Sessions.Any()).ToList();
            }

            var model = new MovieFilterModel
            {
                SelectedGenre = genre,
                Genres = uniqueGenres,
                Movies = movies.ToList(),
                HasSessions = hasSessions.GetValueOrDefault()
            };

            return View(model);
        }
    }
}

