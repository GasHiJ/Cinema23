using System;

namespace Cinema.Core.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public int HallId { get; set; }
        public decimal Price { get; set; }

        public Movie Movie { get; set; }
        public Hall Hall { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
