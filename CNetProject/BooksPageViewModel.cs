namespace CNetProject;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class BooksPageViewModel : BaseViewModel
{
    public ObservableCollection<Book> Books { get; set; }
    public ICommand AddBookCommand { get; }

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
                ShowBookDetails();
            }
        }
    }

    public BooksPageViewModel()
    {
        Books = new ObservableCollection<Book>();
        AddBookCommand = new Command(AddBook);
    }

    private void AddBook()
    {
        Books.Add(new Book { Title = "New Book", Format = "Paperback" });
    }

    private async void ShowBookDetails()
    {
        if (SelectedBook != null)
        {
            await Application.Current.MainPage.DisplayAlert("Book Details",
                $"Title: {SelectedBook.Title}\nFormat: {SelectedBook.Format}",
                "OK");
        }
    }
}