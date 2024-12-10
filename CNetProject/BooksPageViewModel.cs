// BooksPageViewModel.cs

namespace CNetProject;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class BooksPageViewModel : BaseViewModel
{
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
        AddBookCommand = new Command(AddBook);
        OpenAddBooksFormCommand = new Command(OpenAddBookForm);
    }

    private void AddBook()
    {
        Books.Add(new Book { Title = "New Book", Format = "Paperback", Author = "Author", Pages = 100 });
    }

    private async void ShowBookDetails(Book SelectedBook)
    {
        if (SelectedBook != null)
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Book Details",
                $"Title: {SelectedBook.Title}\nFormat: {SelectedBook.Format}",
                "Ok", "Delete");

            if (!result)
            {
                bool confirmDelete = await Application.Current.MainPage.DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete this Book?",
                    "Yes", "No");

                if (confirmDelete)
                {
                    Console.WriteLine($"Deleting Book: {SelectedBook.Title}");
                    Books.Remove(SelectedBook);
                }
            }
        }
    }
    
    private async void OpenAddBookForm()
    {
        await Application.Current.MainPage.Navigation.PushModalAsync(new AddItemPage(false, bookObj =>
        {
            if (bookObj is Book book)
            {
                Books.Add(new Book
                {
                    Title = book.Title,
                    Format = book.Format,
                    Author = book.Author,
                    Pages = book.Pages
                });
            }
        }));
    }
}