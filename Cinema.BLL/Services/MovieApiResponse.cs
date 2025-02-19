using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Services
{
    public class MovieApiResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public int? Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double VoteAverage { get; set; }
        public string PosterPath { get; set; }
        public string TrailerUrl { get; set; }
        public List<Genre> Genres { get; set; }
        public string Actors { get; set; }
    }

    public class Genre
    {
        public string Name { get; set; }
    }

}
