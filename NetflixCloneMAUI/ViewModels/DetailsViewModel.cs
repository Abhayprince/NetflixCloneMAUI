using CommunityToolkit.Mvvm.ComponentModel;
using NetflixCloneMAUI.Models;
using NetflixCloneMAUI.Services;

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

        public async Task InitializeAsync()
        {
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
        }

        private static string GenerateYoutubeUrl(string videoKey) =>
            $"https://www.youtube.com/embed/{videoKey}";
    }
}
