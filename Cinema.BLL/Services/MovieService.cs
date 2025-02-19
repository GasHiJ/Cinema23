using AutoMapper;
using Cinema.BLL.DTOs;
using Cinema.BLL.Services.Interfaces;
using Cinema.Core.Models;
using Cinema.DAL.Repositories.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Cinema.DAL.Repositories;
using Azure;
using Cinema.DAL.DbCon;
using System.Net.Http;
using System.Text.Json;

namespace Cinema.BLL.Services
{

    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITmdbService _tmdbService;

        public MovieService(ITmdbService tmdbService, IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context)
        {
            _tmdbService = tmdbService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }
        public async Task<Movie?> AddMovieByTmdbIdAsync(int tmdbId)
        {
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.TmdbId == tmdbId);
            if (existingMovie != null)
            {
                return existingMovie;
            }

            var movie = await _tmdbService.GetMovieByTmdbIdAsync(tmdbId);
            if (movie == null)
            {
                return null;
            }
            return movie;
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            return movies.Select(movie => new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                Duration = movie.Duration,
                Genre = string.Join(", ", movie.Genre),
                TrailerUrl = movie.TrailerUrl,
                PosterUrl = movie.PosterUrl,
                Rating = movie.Rating,
                ReleaseYear = movie.ReleaseYear,
                TmdbId = movie.TmdbId,
                Actors = movie.Actors
            }).ToList();
        }

        public async Task<MovieDto> GetMovieByIdAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
                return null;

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                Duration = movie.Duration,
                Genre = movie.Genre,
                TrailerUrl = movie.TrailerUrl,
                Rating = movie.Rating,
                PosterUrl = movie.PosterUrl
            };
        }

        public async Task<List<Movie>> GetMoviesSortedByPopularityAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            return movies.OrderByDescending(m => m.Rating).ToList();
        }

        public async Task UpdateMovieAsync(int id, MovieDto movieDto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id) ?? throw new Exception("Фільм не знайдено");
            _mapper.Map(movieDto, movie);
            _unitOfWork.Movies.Update(movie);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            await _unitOfWork.Movies.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<MovieDto>> GetFilteredMoviesAsync(string? genre, int? year, bool onlyWithSessions)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var filteredMovies = movies.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
            {
                filteredMovies = filteredMovies.Where(m => m.Genre.Contains(genre));
            }

            if (year.HasValue)
            {
                filteredMovies = filteredMovies.Where(m => m.ReleaseYear == year.Value);
            }

            if (onlyWithSessions)
            {
                filteredMovies = filteredMovies.Where(m => m.Sessions.Any());
            }

            return _mapper.Map<IEnumerable<MovieDto>>(filteredMovies.ToList());
        }

        public async Task<MovieDto?> GetOrAddMovieByTmdbIdAsync(int tmdbId)
        {
            var existingMovie = await _unitOfWork.Movies.GetByTmdbIdAsync(tmdbId);
            if (existingMovie != null)
            {
                return new MovieDto
                {
                    Id = existingMovie.Id,
                    Title = existingMovie.Title,
                    Description = existingMovie.Description,
                    Director = existingMovie.Director,
                    Duration = existingMovie.Duration,
                    Genre = existingMovie.Genre,
                    TrailerUrl = existingMovie.TrailerUrl,
                    PosterUrl = existingMovie.PosterUrl,
                    Rating = existingMovie.Rating,
                    ReleaseYear = existingMovie.ReleaseYear,
                    TmdbId = existingMovie.TmdbId,
                    Actors = existingMovie.Actors
                };
            }

            var movieData = await _tmdbService.GetMovieByTmdbIdAsync(tmdbId);
            if (movieData == null)
                return null;

            await _unitOfWork.Movies.AddAsync(movieData);
            await _unitOfWork.SaveAsync();

            return new MovieDto
            {
                Id = movieData.Id,
                Title = movieData.Title,
                Description = movieData.Description,
                Director = movieData.Director,
                Duration = movieData.Duration,
                Genre = movieData.Genre,
                TrailerUrl = movieData.TrailerUrl,
                PosterUrl = movieData.PosterUrl,
                Rating = movieData.Rating,
                ReleaseYear = movieData.ReleaseYear,
                TmdbId = movieData.TmdbId,
                Actors = movieData.Actors
            };
        }

    }



}
