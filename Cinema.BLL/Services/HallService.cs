using Cinema.BLL.DTOs;
using Cinema.BLL.Services.Interfaces;
using Cinema.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Services
{
    public class HallService : IHallService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HallService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HallDto>> GetAllHallsAsync()
        {
            var halls = await _unitOfWork.Halls.GetAllAsync();
            return halls.Select(hall => new HallDto
            {
                Id = hall.Id,
                Name = hall.Name,
                Rows = hall.Rows,
                SeatsPerRow = hall.SeatsPerRow
            }).ToList();
        }

        public async Task<HallDto?> GetHallByIdAsync(int hallId)
        {
            var hall = await _unitOfWork.Halls.GetByIdAsync(hallId);
            if (hall == null)
                return null;

            return new HallDto
            {
                Id = hall.Id,
                Name = hall.Name,
                Rows = hall.Rows,
                SeatsPerRow = hall.SeatsPerRow
            };
        }
    }

}
