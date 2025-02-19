using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.BLL.DTOs;

namespace Cinema.BLL.Services.Interfaces
{
    public interface IHallService
    {
        Task<IEnumerable<HallDto>> GetAllHallsAsync();
        Task<HallDto?> GetHallByIdAsync(int hallId);
    }
}
