using NetflixCloneMAUI.ViewModels;

namespace NetflixCloneMAUI.Pages;

public partial class DetailsPage : ContentPage
{
    private readonly DetailsViewModel _viewModel;
    
    public DetailsPage(DetailsViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if(width > 0)
        {
            _viewModel.SimilarItemWidth = Convert.ToInt32(width / 3) - 3;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private void TrailersTab_Tapped(object sender, TappedEventArgs e)
    {
        similarTabIndicator.Color = Colors.Black;
        similarTabContent.IsVisible = false;

        trailersTabIndicator.Color = Colors.Red;
        trailersTabContent.IsVisible = true;
    }

    private void SimilarTab_Tapped(object sender, TappedEventArgs e)
    {
        trailersTabIndicator.Color = Colors.Black;
        trailersTabContent.IsVisible = false;

        similarTabIndicator.Color = Colors.Red;
        similarTabContent.IsVisible = true;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await pageScrollView.ScrollToAsync(0, 0, animated: true);
    }
}