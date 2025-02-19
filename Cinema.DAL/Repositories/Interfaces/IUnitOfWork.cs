using Cinema.Core.Models;

namespace Cinema.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IMovieRepository Movies { get; }
        ISessionRepository Sessions { get; }
        IHallRepository Halls { get; }
        IBookingRepository Bookings { get; }

        Task SaveAsync();
    }

}
