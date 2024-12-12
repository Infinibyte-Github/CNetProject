// BooksPageViewModel.cs

using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CNetProject;

public class BooksPageViewModel : BaseViewModel
{
    private readonly HttpClient _httpClient = new HttpClient();
    public string ApiBaseUrl =>
        DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.0.2:8080/api/books" :
            "http://localhost:8080/api/books";
    
    public ObservableCollection<Book> Books { get; set; }
    public ICommand AddBookCommand { get; }
    public ICommand OpenAddBooksFormCommand { get; }
    
    public ICommand DeleteBookCommand { get; }
    
    public ICommand OpenEditBooksFormCommand { get; }

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
            }
        }
    }

    public BooksPageViewModel()
    {
        Books = new ObservableCollection<Book>();
        AddBookCommand = new Command(async () => await FetchBooks());
        OpenAddBooksFormCommand = new Command(OpenAddBookForm);
        OpenEditBooksFormCommand = new Command(OpenEditBookForm);
        DeleteBookCommand = new Command(async () => await DeleteBook(SelectedBook));

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
            // refresh the UI
            OnPropertyChanged(nameof(Books));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching books: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load books: {ex.Message}", "OK");
        }
    }

    private async Task DeleteBook(Book book)
    {
        if (SelectedBook == null)
        {
            await Application.Current.MainPage.DisplayAlert("Warning", "Please select a book to delete.", "OK");
            return;
        }
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
    
    private async void OpenEditBookForm()
    {
        if (SelectedBook == null)
        {
            await Application.Current.MainPage.DisplayAlert("Warning", "Please select a book to edit.", "OK");
            return;
        }

        string originalTitle = SelectedBook.Title;

        await Application.Current.MainPage.Navigation.PushModalAsync(new EditItemPage(false, updatedBook =>
        {
            if (updatedBook is Book editedBook)
            {
                UpdateBook(editedBook, originalTitle);
            }
        }, SelectedBook));
    }
    
    private async void UpdateBook(Book updatedBook, string originalTitle)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/{originalTitle}", updatedBook);
            if (response.IsSuccessStatusCode)
            {
                await FetchBooks(); // Reload the collection from the API
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"{response}", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating book: {ex.Message}");
        }
    }


}