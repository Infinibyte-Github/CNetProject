namespace CNetProject;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class BooksPageViewModel
{
    public ObservableCollection<Book> Books { get; set; }
    public ICommand AddBookCommand { get; }

    public BooksPageViewModel()
    {
        Books = new ObservableCollection<Book>();
        AddBookCommand = new Command(AddBook);
    }

    private void AddBook()
    {
        Books.Add(new Book { Title = "New Book", Format = "Paperback" });
    }
}