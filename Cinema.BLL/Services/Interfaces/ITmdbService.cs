using Cinema.BLL.DTOs;
using Cinema.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Services.Interfaces
{
    public interface ITmdbService
    {
        Task<Movie?> FetchMovieDataAsync(int tmdbId);
        Task<Movie?> GetMovieByTmdbIdAsync(int tmdbId);
    }

}
