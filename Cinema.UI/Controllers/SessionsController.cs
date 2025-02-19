using Cinema.BLL.DTOs;
using Cinema.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cinema.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetAllSessions()
        {
            var sessions = await _sessionService.GetAllSessionsAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDto>> GetSessionById(int id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var userId))
            {
                TempData["Error"] = "Не вдалося отримати ваш ідентифікатор.";
                return RedirectToAction("Index", "Booking");
            }

            var session = await _sessionService.GetSessionByIdAsync(id, userId);
            if (session == null)
            {
                TempData["Error"] = "Сесія не знайдена.";
                return RedirectToAction("Index", "Booking");
            }

            return Ok(session);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessionsByMovieId(int movieId)
        {
            var sessions = await _sessionService.GetSessionsByMovieIdAsync(movieId);
            return Ok(sessions);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessionsByDate(DateTime date)
        {
            var sessions = await _sessionService.GetSessionsByDateAsync(date);
            return Ok(sessions);
        }

        [HttpPost]
        public async Task<ActionResult<SessionDto>> CreateSession(SessionDto sessionDto)
        {
            var session = await _sessionService.CreateSessionAsync(sessionDto);
            return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, session);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var deleted = await _sessionService.DeleteSessionAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

}
