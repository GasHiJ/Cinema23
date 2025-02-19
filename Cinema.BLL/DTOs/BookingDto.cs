using Cinema.Core.Models;

namespace Cinema.BLL.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MovieTitle { get; set; }
        public int SessionId { get; set; }
        public DateTime SessionStartTime { get; set; }
        public string HallName { get; set; }
        public int Row { get; set; }
        public int Seat { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public Session Session { get; set; }
    }

}
