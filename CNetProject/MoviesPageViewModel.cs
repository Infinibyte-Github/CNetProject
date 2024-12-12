// MoviesPageViewModel.cs

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
    public ICommand OpenAddMoviesFormCommand { get; }
    
    public ICommand DeleteMovieCommand { get; }
    
    public ICommand OpenEditMoviesFormCommand { get; }

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
            }
        }
    }

    public MoviesPageViewModel()
    {
        Movies = new ObservableCollection<Movie>();
        AddMovieCommand = new Command(async () => await FetchMovies());
        OpenAddMoviesFormCommand = new Command(OpenAddMovieForm);
        OpenEditMoviesFormCommand = new Command(OpenEditMovieForm);
        DeleteMovieCommand = new Command(async () => await DeleteMovie(SelectedMovie));

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
                    Movies.Add(movie);
                }
            }
            // refresh the UI
            OnPropertyChanged(nameof(Movies));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching movies: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load movies: {ex.Message}", "OK");
        }
    }

    private async Task DeleteMovie(Movie movie)
    {
        if (SelectedMovie == null)
        {
            await Application.Current.MainPage.DisplayAlert("Warning", "Please select a movie to delete.", "OK");
            return;
        }
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
    
    private async void OpenEditMovieForm()
    {
        if (SelectedMovie == null)
        {
            await Application.Current.MainPage.DisplayAlert("Warning", "Please select a movie to edit.", "OK");
            return;
        }

        string originalTitle = SelectedMovie.Title;

        await Application.Current.MainPage.Navigation.PushModalAsync(new EditItemPage(true, updatedMovie =>
        {
            if (updatedMovie is Movie movie)
            {
                UpdateMovie(movie, originalTitle);
            }
        }, null, SelectedMovie));
    }
    
    private async void UpdateMovie(Movie updatedMovie, string originalTitle)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/{originalTitle}", updatedMovie);
            if (response.IsSuccessStatusCode)
            {
                await FetchMovies(); // Reload the collection from the API
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"{response}", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating movie: {ex.Message}");
        }
    }
}