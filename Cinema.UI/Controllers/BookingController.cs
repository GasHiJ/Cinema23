using Cinema.BLL.DTOs;
using Cinema.BLL.Services.Interfaces;
using Cinema.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Cinema.Core.Models;

namespace Cinema.UI.Controllers
{
    [Route("bookings")]
    public class BookingController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IBookingService _bookingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingController> _logger;

        public BookingController(ISessionService sessionService, ILogger<BookingController> logger, IBookingService bookingService, IUnitOfWork unitOfWork)
        {
            _sessionService = sessionService;
            _bookingService = bookingService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? movieTitle, DateTime? startDate)
        {
            var sessions = await _sessionService.GetAllSessionsAsync();

            if (!string.IsNullOrEmpty(movieTitle))
            {
                sessions = sessions.Where(s => s.MovieTitle.Contains(movieTitle, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (startDate.HasValue)
            {
                sessions = sessions.Where(s => s.StartTime.Date == startDate.Value.Date).ToList();
            }

            return View(sessions);
        }


        [HttpPost]
        public async Task<IActionResult> Book(int sessionId, int row, int seat)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var userId))
            {
                TempData["Error"] = "Не вдалося отримати ваш ідентифікатор.";
                return RedirectToAction("Create", new { sessionId });
            }

            var session = await _sessionService.GetSessionByIdAsync(sessionId, userId);
            if (session == null)
            {
                TempData["Error"] = "Ця сесія не існує або була видалена.";
                return RedirectToAction("Create", new { sessionId });
            }

            await _bookingService.CreateBookingAsync(sessionId, userId, row, seat);

            TempData["Success"] = "Бронювання успішне!";
            return RedirectToAction("Create", new { sessionId });
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(int sessionId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var userId))
            {
                TempData["Error"] = "Не вдалося отримати ваш ідентифікатор.";
                return RedirectToAction("Index");
            }

            var session = await _sessionService.GetSessionByIdAsync(sessionId, userId);
            if (session == null)
            {
                TempData["Error"] = "Ця сесія не існує або була видалена.";
                return RedirectToAction("Index");
            }

            return View(session);
        }

        public async Task<IActionResult> Details(int id)
        {
            var session = await _unitOfWork.Sessions
                .GetByIdAsync(id);

            if (session == null)
                return NotFound();

            return View(session);
        }
    }
}
