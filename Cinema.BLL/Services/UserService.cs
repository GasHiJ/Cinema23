using Cinema.BLL.DTOs;
using Cinema.DAL.Repositories.Interfaces;
using Cinema.Core.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cinema.BLL.Services.Interfaces;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Cinema.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : new UserDto { Id = user.Id, Email = user.Email, Role = user.Role };
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : new UserDto { Id = user.Id, Email = user.Email, Role = user.Role };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("Користувач із таким email вже існує");
            }

            var hashedPassword = HashPassword(registerDto.Password);
            var user = new User { Email = registerDto.Email, PasswordHash = hashedPassword, Role = "Customer" };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();

            return new UserDto { Id = user.Id, Email = user.Email, Role = user.Role };
        }


        public async Task<UserDto?> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };
            _logger.LogInformation("Користувач {Email} аутентифікований з Id: {UserId}", user.Email, userDto.Id);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Email),
                new Claim(ClaimTypes.Role, userDto.Role),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);

            return userDto;
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string inputPassword, string storedHash)
        {
            return HashPassword(inputPassword) == storedHash;
        }
    }
}
