using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CNetProject;

public class BooksPageViewModel : BaseViewModel
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const string ApiBaseUrl = "http://localhost:8080/api/books";

    public ObservableCollection<Book> Books { get; set; }
    public ICommand AddBookCommand { get; }
    public ICommand OpenAddBooksFormCommand { get; }

    private Book _selectedBook;
    public Book SelectedBook
    {
        get => _selectedBook;
        set
        {
            if (_selectedBook != value)
            {
                _selectedBook = value;
                OnPropertyChanged();
                ShowBookDetails(_selectedBook);
            }
        }
    }

    public BooksPageViewModel()
    {
        Books = new ObservableCollection<Book>();
        AddBookCommand = new Command(async () => await FetchBooks());
        OpenAddBooksFormCommand = new Command(OpenAddBookForm);

        // Load books from API when the page is loaded
        FetchBooks();
    }

    private async Task FetchBooks()
    {
        try
        {
            var booksFromApi = await _httpClient.GetFromJsonAsync<List<Book>>(ApiBaseUrl);
            Books.Clear();  // Clear the existing list
            if (booksFromApi != null)
            {
                foreach (var book in booksFromApi)
                {
                    Books.Add(book);  // Add all fetched books to the ObservableCollection
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching books: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load books: {ex.Message}", "OK");
        }
    }

    private async void ShowBookDetails(Book selectedBook)
    {
        if (selectedBook != null)
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Book Details",
                $"Title: {selectedBook.Title}\nFormat: {selectedBook.Format}",
                "Ok", "Delete");

            if (!result)
            {
                bool confirmDelete = await Application.Current.MainPage.DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete this book?",
                    "Yes", "No");

                if (confirmDelete)
                {
                    await DeleteBook(selectedBook);
                }
            }
        }
    }

    private async Task DeleteBook(Book book)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{book.Title}");
            if (response.IsSuccessStatusCode)
            {
                Books.Remove(book);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete the book.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting book: {ex.Message}");
        }
    }

    private async void OpenAddBookForm()
    {
        await Application.Current.MainPage.Navigation.PushModalAsync(new AddItemPage(false, async bookObj =>
        {
            if (bookObj is Book book)
            {
                await AddBook(book);
            }
        }));
    }

    private async Task AddBook(Book newBook)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiBaseUrl, newBook);
            if (response.IsSuccessStatusCode)
            {
                Books.Add(newBook);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to add the book.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding book: {ex.Message}");
        }
    }
}