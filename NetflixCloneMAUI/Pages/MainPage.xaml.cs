using NetflixCloneMAUI.Services;

namespace NetflixCloneMAUI.Pages;

public partial class MainPage : ContentPage
{
    private readonly TmdbService _tmdbService;
    int count = 0;

	public MainPage(TmdbService tmdbService)
	{
		InitializeComponent();
        _tmdbService = tmdbService;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
		var trending = await _tmdbService.GetTrendingAsync();
    }

    private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

