namespace Cinema.Core.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }
        public List<Session> Sessions { get; set; } = new();
    }
}
