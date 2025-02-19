using Cinema.BLL.DTOs;
using Cinema.Core.Models;
using Cinema.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionDto>> GetAllSessionsAsync();
        Task<SessionDto> GetSessionByIdAsync(int sessionId, int userId);
        Task<IEnumerable<SessionDto>> GetSessionsByMovieIdAsync(int movieId);
        Task<IEnumerable<SessionDto>> GetSessionsByDateAsync(DateTime date);
        Task<SessionDto> CreateSessionAsync(SessionDto sessionDto);
        Task<bool> DeleteSessionAsync(int id);
        Task CreateSessionAsync(CreateSessionDto sessionDto);

    }
}
