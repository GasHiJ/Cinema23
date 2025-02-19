namespace Cinema.Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int Row { get; set; }
        public int Seat { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public Session Session { get; set; }
    }


}
