namespace Cinema.BLL.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string TrailerUrl { get; set; }
        public string PosterUrl { get; set; }
        public double Rating { get; set; }
        public int ReleaseYear { get; set; }
        public int TmdbId { get; set; }
        public string Actors { get; set; }
        public List<SessionDto> Sessions { get; set; }
    }        


}
