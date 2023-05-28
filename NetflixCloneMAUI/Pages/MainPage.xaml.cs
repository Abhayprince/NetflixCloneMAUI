using NetflixCloneMAUI.ViewModels;

namespace NetflixCloneMAUI.Pages;

public partial class MainPage : ContentPage
{
    private readonly HomeViewModel _homeViewModel;
	public MainPage(HomeViewModel homeViewModel)
	{
		InitializeComponent();
        _homeViewModel = homeViewModel;
		BindingContext = _homeViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
		await _homeViewModel.InitializeAsync();
    }

    private void MovieRow_MediaSelected(object sender, Controls.MediaSelectEventArgs e)
    {
        _homeViewModel.SelectMediaCommand.Execute(e.Media);
    }

    private void MovieInfoBox_Closed(object sender, EventArgs e)
    {
        _homeViewModel.SelectMediaCommand.Execute(null);
    }

    private async void CategoriesMenu_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CategoriesPage));
    }
}

