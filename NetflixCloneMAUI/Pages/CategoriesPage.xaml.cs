using NetflixCloneMAUI.ViewModels;

namespace NetflixCloneMAUI.Pages;

public partial class CategoriesPage : ContentPage
{
    private readonly CategoriesViewModel _categoriesViewModel;

    public CategoriesPage(CategoriesViewModel categoriesViewModel)
	{
		InitializeComponent();
        _categoriesViewModel = categoriesViewModel;
        BindingContext = _categoriesViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _categoriesViewModel.InitializeAsync();
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
}