using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetflixCloneMAUI.Models;
using NetflixCloneMAUI.Pages;
using NetflixCloneMAUI.Services;
using System.Collections.ObjectModel;

namespace NetflixCloneMAUI.ViewModels
{
    [QueryProperty(nameof(Media), nameof(Media))]
    public partial class DetailsViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;

        public DetailsViewModel(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [ObservableProperty]
        private Media _media;

        [ObservableProperty]
        private string _mainTrailerUrl;

        [ObservableProperty]
        private int _runtime;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _similarItemWidth = 125;

        public ObservableCollection<Video> Videos { get; set; } = new();
        public ObservableCollection<Media> Similar { get; set; } = new();

        public async Task InitializeAsync()
        {
            var similarMediasTask = _tmdbService.GetSimilarAsync(Media.Id, Media.MediaType);
            IsBusy = true;
            try
            {
                var trailerTeasersTask = _tmdbService.GetTrailersAsync(Media.Id, Media.MediaType);
                var detailsTask = _tmdbService.GetMediaDetailsAsync(Media.Id, Media.MediaType);

                var trailerTeasers = await trailerTeasersTask;
                var details = await detailsTask;

                if (trailerTeasers?.Any() == true)
                {
                    var trailer = trailerTeasers.FirstOrDefault(t => t.type == "Trailer");
                    trailer ??= trailerTeasers.First();
                    MainTrailerUrl = GenerateYoutubeUrl(trailer.key);

                    foreach (var video in trailerTeasers)
                    {
                        Videos.Add(video);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Not found", "No videos found", "Ok");
                }
                if (details is not null)
                {
                    Runtime = details.runtime;
                }
            }
            finally
            {
                IsBusy = false; 
            }

            var similarMedias = await similarMediasTask;
            if(similarMedias?.Any() == true)
            {
                foreach (var media in similarMedias)
                {
                    Similar.Add(media);
                }
            }
        }

        [RelayCommand]
        private async Task ChangeToThisMedia(Media media)
        {
            var parameters = new Dictionary<string, object>
            {
                [nameof(DetailsViewModel.Media)] = media
            };
            await Shell.Current.GoToAsync(nameof(DetailsPage), true, parameters);
        }

        [RelayCommand]
        private void SetMainTrailer(string videoKey) => 
            MainTrailerUrl = GenerateYoutubeUrl(videoKey);

        private static string GenerateYoutubeUrl(string videoKey) =>
            $"https://www.youtube.com/embed/{videoKey}";
    }
}
