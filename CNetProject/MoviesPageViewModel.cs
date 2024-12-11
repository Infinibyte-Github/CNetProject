using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CNetProject;

public class MoviesPageViewModel : BaseViewModel
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const string ApiBaseUrl = "http://localhost:8080/api/movies";

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
        AddMovieCommand = new Command(async () => await FetchMovies());
        OpenAddMovieFormCommand = new Command(OpenAddMovieForm);

        // Load movies from API when the page is loaded
        FetchMovies();
    }

    private async Task FetchMovies()
    {
        try
        {
            var moviesFromApi = await _httpClient.GetFromJsonAsync<List<Movie>>(ApiBaseUrl);
            Movies.Clear();  // Clear the existing list
            if (moviesFromApi != null)
            {
                foreach (var movie in moviesFromApi)
                {
                    Movies.Add(movie);  // Add all fetched movies to the ObservableCollection
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching movies: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load movies: {ex.Message}", "OK");
        }
    }


    private async void ShowMovieDetails(Movie selectedMovie)
    {
        if (selectedMovie != null)
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Movie Details",
                $"Title: {selectedMovie.Title}\nFormat: {selectedMovie.Format}",
                "Ok", "Delete");

            if (!result)
            {
                bool confirmDelete = await Application.Current.MainPage.DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete this movie?",
                    "Yes", "No");

                if (confirmDelete)
                {
                    await DeleteMovie(selectedMovie);
                }
            }
        }
    }

    private async Task DeleteMovie(Movie movie)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{movie.Title}");
            if (response.IsSuccessStatusCode)
            {
                Movies.Remove(movie);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete the movie.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting movie: {ex.Message}");
        }
    }

    private async void OpenAddMovieForm()
    {
        await Application.Current.MainPage.Navigation.PushModalAsync(new AddItemPage(true, async movieObj =>
        {
            if (movieObj is Movie movie)
            {
                await AddMovie(movie);
            }
        }));
    }

    private async Task AddMovie(Movie newMovie)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiBaseUrl, newMovie);
            if (response.IsSuccessStatusCode)
            {
                Movies.Add(newMovie);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to add the movie.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding movie: {ex.Message}");
        }
    }
}
