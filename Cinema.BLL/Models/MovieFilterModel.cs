using Cinema.BLL.DTOs;
using Cinema.Core.Models;

namespace Cinema.BLL.Models
{
    public class MovieFilterModel
    {
        public string? SelectedGenre { get; set; }
        public bool HasSessions { get; set; }
        public List<string> Genres { get; set; }
        public List<MovieDto> Movies { get; set; } = new();
    }
}
