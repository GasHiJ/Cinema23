using Cinema.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Models
{
    public class AdminViewModel
    {
        public IEnumerable<MovieDto> Movies { get; set; } = new List<MovieDto>();
        public IEnumerable<HallDto> Halls { get; set; } = new List<HallDto>();
    }
}
