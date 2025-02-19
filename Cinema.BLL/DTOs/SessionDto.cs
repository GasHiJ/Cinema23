namespace Cinema.BLL.DTOs
{
    public class SessionDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public DateTime StartTime { get; set; }
        public string HallName { get; set; }
        public decimal Price { get; set; }
        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }
        public List<SeatDto> BookedSeats { get; set; } = new();
        public int CurrentUserId { get; set; }
    }


}
