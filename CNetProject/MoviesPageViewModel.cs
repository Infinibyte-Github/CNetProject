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
                ShowMovieDetails();
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
    
    private async void OpenAddMovieForm()
    {
        await Application.Current.MainPage.Navigation.PushModalAsync(new AddItemPage((title, format) =>
        {
            Movies.Add(new Movie { Title = title, Format = format });
        }));
    }
}