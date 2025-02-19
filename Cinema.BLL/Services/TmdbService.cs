using AutoMapper;
using Cinema.BLL.DTOs;
using Cinema.BLL.Services.Interfaces;
using Cinema.Core.Models;
using Cinema.DAL.Repositories.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;


namespace Cinema.BLL.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TmdbApiKey"] ?? throw new ArgumentNullException("API key is missing");
        }
        public async Task<Movie?> FetchMovieDataAsync(int tmdbId)
        {
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key=0fd2a71e0e55ce6f96af434a700d178f";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<MovieApiResponse>(content);

            var genre = data?.Genres?.FirstOrDefault()?.Name ?? string.Empty;

            return new Movie
            {
                TmdbId = data.Id,
                Title = data.Title,
                Description = data.Overview,
                Duration = data.Runtime ?? 0,
                ReleaseYear = data.ReleaseDate.Year,
                Rating = data.VoteAverage,
                PosterUrl = data.PosterPath,
                TrailerUrl = data.TrailerUrl,
                Genre = genre,
            };
        }

        public async Task<Movie?> GetMovieByTmdbIdAsync(int tmdbId)
        {
            var url = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={_apiKey}&append_to_response=videos,credits";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var movieData = JsonSerializer.Deserialize<TmdbMovieResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (movieData == null)
            {
                return null;
            }

            var genre = movieData.Genres?.FirstOrDefault()?.Name ?? string.Empty;

            return new Movie
            {
                TmdbId = tmdbId,
                Title = movieData.Title,
                Description = movieData.Overview,
                Director = movieData.GetDirector(),
                Duration = movieData.Runtime,
                Genre = genre,
                TrailerUrl = movieData.GetTrailerUrl(),
                PosterUrl = $"https://image.tmdb.org/t/p/w500{movieData.PosterPath}",
                Rating = movieData.VoteAverage,
                ReleaseYear = DateTime.Parse(movieData.ReleaseDate).Year,
                Actors = movieData.GetMainActors()
            };
        }
    }
    public class TmdbMovieResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("overview")]
        public string Overview { get; set; } = "";

        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = "";

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = "";

        [JsonPropertyName("genres")]
        public List<TmdbGenre> Genres { get; set; } = new();

        [JsonPropertyName("videos")]
        public TmdbVideos Videos { get; set; } = new();

        [JsonPropertyName("credits")]
        public TmdbCredits Credits { get; set; } = new();

        public string GetDirector()
        {
            return Credits.Crew.FirstOrDefault(p => p.Job == "Director")?.Name ?? "";
        }

        public List<string> GetGenres()
        {
            return Genres.Any() ? Genres.Select(g => g.Name).ToList() : new List<string>();
        }

        public string GetTrailerUrl()
        {
            var trailer = Videos.Results.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube");
            return trailer != null ? $"https://www.youtube.com/watch?v={trailer.Key}" : "";
        }

        public string GetMainActors()
        {
            return string.Join(", ", Credits.Cast.Take(3).Select(a => a.Name));
        }
    }

    public class TmdbGenre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public class TmdbVideos
    {
        [JsonPropertyName("results")]
        public List<TmdbVideo> Results { get; set; } = new();
    }

    public class TmdbVideo
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = "";

        [JsonPropertyName("site")]
        public string Site { get; set; } = "";

        [JsonPropertyName("type")]
        public string Type { get; set; } = "";
    }

    public class TmdbCredits
    {
        [JsonPropertyName("cast")]
        public List<TmdbCast> Cast { get; set; } = new();

        [JsonPropertyName("crew")]
        public List<TmdbCrew> Crew { get; set; } = new();
    }

    public class TmdbCast
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public class TmdbCrew
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("job")]
        public string Job { get; set; } = "";
    }

}