// MoviesPageViewModel.cs

namespace CNetProject;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class MoviesPageViewModel : BaseViewModel
{
    public ObservableCollection<Movie> Movies { get; set; }
    public ICommand AddMovieCommand { get; }
    public ICommand OpenAddMovieFormCommand { get; }

    private Movie _selectedMovie;
    public Movie SelectedMovie
    {
        get => _selectedMovie;
        set
        {
            if (_selectedMovie != value)
            {
                _selectedMovie = value;
                OnPropertyChanged();
                ShowMovieDetails(_selectedMovie);
            }
        }
    }

    public MoviesPageViewModel()
    {
        Movies = new ObservableCollection<Movie>();
        AddMovieCommand = new Command(AddMovie);
        OpenAddMovieFormCommand = new Command(OpenAddMovieForm);
    }

    private void AddMovie()
    {
        Movies.Add(new Movie { Title = "New Movie", Format = "DVD", Director = "Director", ReleaseYear = 2024 });
    }

    private async void ShowMovieDetails(Movie SelectedMovie)
    {
        if (SelectedMovie != null)
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Movie Details",
                $"Title: {SelectedMovie.Title}\nFormat: {SelectedMovie.Format}",
                "Ok", "Delete");

            if (!result)
            {
                bool confirmDelete = await Application.Current.MainPage.DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete this movie?",
                    "Yes", "No");

                if (confirmDelete)
                {
                    Console.WriteLine($"Deleting movie: {SelectedMovie.Title}");
                    Movies.Remove(SelectedMovie);
                }
            }
        }
    }

    private async void OpenAddMovieForm()
    {
        await Application.Current.MainPage.Navigation.PushModalAsync(new AddItemPage(true, movieObj =>
        {
            if (movieObj is Movie movie)
            {
                Movies.Add(new Movie
                {
                    Title = movie.Title,
                    Format = movie.Format,
                    Director = movie.Director,
                    ReleaseYear = movie.ReleaseYear
                });
            }
        }));
    }
}