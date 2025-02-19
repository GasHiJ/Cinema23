using Cinema.BLL.Models;
using Cinema.BLL.Services.Interfaces;
using Cinema.BLL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cinema.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ISessionService _sessionService;
        private readonly IHallService _hallService;

        public AdminController(IMovieService movieService, ISessionService sessionService, IHallService hallService)
        {
            _movieService = movieService;
            _sessionService = sessionService;
            _hallService = hallService;
        }

        public async Task<IActionResult> Index()
        {
            var halls = await _hallService.GetAllHallsAsync();

            var model = new AdminViewModel
            {
                Halls = halls
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(int tmdbId)
        {
            if (tmdbId <= 0)
            {
                TempData["Error"] = "Некоректний TMDb ID.";
                return RedirectToAction("Index");
            }

            var movie = await _movieService.GetOrAddMovieByTmdbIdAsync(tmdbId);
            if (movie == null)
            {
                TempData["Error"] = "Фільм не знайдено або не вдалося додати.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Фільм додано успішно!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(int tmdbId, int hallId, DateTime startTime, decimal price)
        {
            if (tmdbId <= 0 || hallId <= 0 || price <= 0)
            {
                TempData["Error"] = "Некоректні дані.";
                return RedirectToAction("Index");
            }

            var movie = await _movieService.GetOrAddMovieByTmdbIdAsync(tmdbId);
            if (movie == null)
            {
                TempData["Error"] = "Фільм не знайдено.";
                return RedirectToAction("Index");
            }

            var session = new SessionDto
            {
                MovieId = movie.Id,
                HallName = (await _hallService.GetHallByIdAsync(hallId))?.Name ?? "Невідомий зал",
                StartTime = startTime,
                Price = price
            };

            await _sessionService.CreateSessionAsync(session);

            TempData["Success"] = "Сеанс створено успішно!";
            return RedirectToAction("Index");
        }
    }
}
