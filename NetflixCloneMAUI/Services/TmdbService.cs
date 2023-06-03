using NetflixCloneMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NetflixCloneMAUI.Services
{
    public partial class TmdbService
    {
        //private const string ApiKey = "738b6d8d99b51339705b1037c981a488"; // generate it from tmdb website
        public const string TmdbHttpClientName = "TmdbClient";

        private readonly IHttpClientFactory _httpClientFactory;

        public TmdbService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient HttpClient => _httpClientFactory.CreateClient(TmdbHttpClientName);

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            var genresWrapper = await HttpClient.GetFromJsonAsync<GenreWrapper>($"{TmdbUrls.MovieGenres}&api_key={ApiKey}");
            return genresWrapper.Genres;
        }   

        public async Task<IEnumerable<Media>> GetTrendingAsync() =>
            await GetMediasAsync(TmdbUrls.Trending);

        public async Task<IEnumerable<Media>> GetTopRatedAsync() =>
            await GetMediasAsync(TmdbUrls.TopRated);
        public async Task<IEnumerable<Media>> GetNetflixOriginalAsync() =>
            await GetMediasAsync(TmdbUrls.NetflixOriginals);
        public async Task<IEnumerable<Media>> GetActionAsync() =>
            await GetMediasAsync(TmdbUrls.Action);

        public async Task<IEnumerable<Video>?> GetTrailersAsync(int id, string type = "movie")
        {
            var videosWrapper = await HttpClient.GetFromJsonAsync<VideosWrapper>(
                $"{TmdbUrls.GetTrailers(id, type)}&api_key={ApiKey}");

            if(videosWrapper?.results?.Length > 0)
            {
                var trailerTeasers = videosWrapper.results.Where(VideosWrapper.FilterTrailerTeasers);
                return trailerTeasers;
            }
            return null;
        }

        public async Task<MovieDetail> GetMediaDetailsAsync(int id, string type = "movie") =>
            await HttpClient.GetFromJsonAsync<MovieDetail>(
                $"{TmdbUrls.GetMovieDetails(id, type)}&api_key={ApiKey}");

        public async Task<IEnumerable<Media>> GetSimilarAsync(int id, string type = "movie") =>
            await GetMediasAsync(
                $"{TmdbUrls.GetSimilar(id, type)}&api_key={ApiKey}");

        private async Task<IEnumerable<Media>> GetMediasAsync(string url)
        {
            var trendingMoviesCollection = await HttpClient.GetFromJsonAsync<Movie>($"{url}&api_key={ApiKey}");
            return trendingMoviesCollection.results
                    .Select(r => r.ToMediaObject());
        }
    }
    public static class TmdbUrls
    {
        public const string Trending = "3/trending/all/week?language=en-US";
        public const string NetflixOriginals = "3/discover/tv?language=en-US&with_networks=213";
        public const string TopRated = "3/movie/top_rated?language=en-US";
        public const string Action = "3/discover/movie?language=en-US&with_genres=28";
        public const string MovieGenres = "3/genre/movie/list?language=en-US";

        public static string GetTrailers(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}/videos?language=en-US";
        public static string GetMovieDetails(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}?language=en-US";
        public static string GetSimilar(int movieId, string type = "movie") => $"3/{type ?? "movie"}/{movieId}/similar?language=en-US";
    }

    public class Movie
    {
        public int page { get; set; }
        public Result[] results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }

    public class Result
    {
        public string backdrop_path { get; set; }
        public int[] genre_ids { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public bool video { get; set; }
        public string media_type { get; set; } // "movie" or "tv"
        public string ThumbnailPath => poster_path ?? backdrop_path;
        public string Thumbnail => $"https://image.tmdb.org/t/p/w600_and_h900_bestv2/{ThumbnailPath}";
        public string ThumbnailSmall => $"https://image.tmdb.org/t/p/w220_and_h330_face/{ThumbnailPath}";
        public string ThumbnailUrl => $"https://image.tmdb.org/t/p/original/{ThumbnailPath}";
        public string DisplayTitle => title ?? name ?? original_title ?? original_name;

        public Media ToMediaObject() =>
            new ()
            {
                Id = id,
                DisplayTitle = DisplayTitle,
                MediaType = media_type,
                Overview = overview,
                ReleaseDate = release_date,
                Thumbnail = Thumbnail,
                ThumbnailSmall = ThumbnailSmall,
                ThumbnailUrl = ThumbnailUrl
            };
    }


    public class VideosWrapper
    {
        public int id { get; set; }
        public Video[] results { get; set; }

        public static Func<Video, bool> FilterTrailerTeasers => v =>
            v.official
            && v.site.Equals("Youtube", StringComparison.OrdinalIgnoreCase)
            && (v.type.Equals("Teaser", StringComparison.OrdinalIgnoreCase) || v.type.Equals("Trailer", StringComparison.OrdinalIgnoreCase));
    }

    public class Video
    {
        public string name { get; set; }
        public string key { get; set; }
        public string site { get; set; }
        public string type { get; set; }
        public bool official { get; set; }
        public DateTime published_at { get; set; }
        public string Thumbnail => $"https://i.ytimg.com/vi/{key}/mqdefault.jpg";
    }


    public class MovieDetail
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public object belongs_to_collection { get; set; }
        public int budget { get; set; }
        public Genre[] genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public Production_Companies[] production_companies { get; set; }
        public Production_Countries[] production_countries { get; set; }
        public string release_date { get; set; }
        public int revenue { get; set; }
        public int runtime { get; set; }
        public Spoken_Languages[] spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
    }

    public class Production_Companies
    {
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }

    public class Production_Countries
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

    public class Spoken_Languages
    {
        public string english_name { get; set; }
        public string iso_639_1 { get; set; }
        public string name { get; set; }
    }
    public class GenreWrapper
    {
        public IEnumerable<Genre> Genres { get; set; }
    }
    public record struct Genre(int Id, string Name);
}
