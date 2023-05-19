using CommunityToolkit.Mvvm.ComponentModel;
using NetflixCloneMAUI.Models;
using NetflixCloneMAUI.Services;
using System.Collections.ObjectModel;

namespace NetflixCloneMAUI.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;
        public HomeViewModel(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [ObservableProperty]
        private Media _trendingMovie;

        [ObservableProperty]
        private Media _selectedMedia;

        public ObservableCollection<Media> Trending { get; set; } = new();
        public ObservableCollection<Media> TopRated { get; set; } = new();
        public ObservableCollection<Media> NetflixOriginals { get; set; } = new();
        public ObservableCollection<Media> ActionMovies { get; set; } = new();

        public async Task InitializeAsync()
        {
            var trendingListTask = _tmdbService.GetTrendingAsync();
            var netflixOriginalsListTask = _tmdbService.GetNetflixOriginalAsync();
            var topRatedListTask = _tmdbService.GetTopRatedAsync();
            var actionListTask = _tmdbService.GetActionAsync();

            var medias = await Task.WhenAll(trendingListTask,
                                    netflixOriginalsListTask,
                                    topRatedListTask,
                                    actionListTask);

            var trendingList = medias[0];
            var netflixOriginalsList = medias[1];
            var topRatedList = medias[2];
            var actionList = medias[3];

            // Seting random trending movie from Trending List to the Trending Movie
            TrendingMovie = trendingList.OrderBy(t => Guid.NewGuid())
                                .FirstOrDefault(t =>
                                    !string.IsNullOrWhiteSpace(t.DisplayTitle)
                                    && !string.IsNullOrWhiteSpace(t.Thumbnail));

            SetMediaCollection(trendingList, Trending);
            SetMediaCollection(netflixOriginalsList, NetflixOriginals);
            SetMediaCollection(topRatedList, TopRated);
            SetMediaCollection(actionList, ActionMovies);

            SelectedMedia = TrendingMovie;
        }

        private static void SetMediaCollection(IEnumerable<Media> medias, ObservableCollection<Media> collection)
        {
            collection.Clear();
            foreach (var media in medias)
            {
                collection.Add(media);
            }
        }
    }
}
