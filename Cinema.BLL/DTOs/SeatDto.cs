using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.DTOs
{
    public class SeatDto
    {
        public int Row { get; set; }
        public int Seat { get; set; }
        public int? UserId { get; set; }
        public bool IsCurrentUserBooking { get; set; }
    }

}
