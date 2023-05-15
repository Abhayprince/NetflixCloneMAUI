using NetflixCloneMAUI.Models;

namespace NetflixCloneMAUI.Controls;

public partial class MovieRow : ContentView
{
	public static readonly BindableProperty HeadingProperty =
			BindableProperty.Create(nameof(Heading), typeof(string), typeof(MovieRow), string.Empty);

	public static readonly BindableProperty MoviesProperty =
			BindableProperty.Create(nameof(Movies), typeof(IEnumerable<Media>), typeof(MovieRow), Enumerable.Empty<Media>());

	public static readonly BindableProperty IsLargeProperty =
			BindableProperty.Create(nameof(IsLarge), typeof(bool), typeof(MovieRow), false);
	
	public MovieRow()
	{
		InitializeComponent();
	}


	public string Heading
    {
		get => (string)GetValue(MovieRow.HeadingProperty);
		set => SetValue(MovieRow.HeadingProperty, value);
	}
	public IEnumerable<Media> Movies
    {
		get => (IEnumerable<Media>)GetValue(MovieRow.MoviesProperty);
		set => SetValue(MovieRow.MoviesProperty, value);
	}
	public bool IsLarge
    {
		get => (bool)GetValue(MovieRow.IsLargeProperty);
		set => SetValue(MovieRow.IsLargeProperty, value);
	}

	public bool IsNotLarge => !IsLarge;
}