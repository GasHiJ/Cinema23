using Cinema.BLL.DTOs;
using System.Threading.Tasks;

namespace Cinema.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto?> AuthenticateAsync(LoginDto loginDto);
        Task LogoutAsync();
    }
}
