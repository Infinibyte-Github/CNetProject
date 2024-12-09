namespace CNetProject;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class MoviesPageViewModel : BaseViewModel
{
    public ObservableCollection<Movie> Movies { get; set; }
    public ICommand AddMovieCommand { get; }

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
                ShowMovieDetails();
            }
        }
    }

    public MoviesPageViewModel()
    {
        Movies = new ObservableCollection<Movie>();
        AddMovieCommand = new Command(AddMovie);
    }

    private void AddMovie()
    {
        Movies.Add(new Movie { Title = "New Movie", Format = "DVD" });
    }

    private async void ShowMovieDetails()
    {
        if (SelectedMovie != null)
        {
            await Application.Current.MainPage.DisplayAlert("Movie Details",
                $"Title: {SelectedMovie.Title}\nFormat: {SelectedMovie.Format}",
                "OK");
        }
    }
}