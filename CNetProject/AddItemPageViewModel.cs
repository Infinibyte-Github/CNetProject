// AddItemPageViewModel.cs

namespace CNetProject;

using System.Collections.ObjectModel;
using System.Windows.Input;

public class AddItemPageViewModel : BaseViewModel
{
    private readonly Action<object> _onSave;
    private readonly bool _isMovie;

    public string PageTitle => _isMovie ? "Add New Movie" : "Add New Book";

    // Common Fields
    public string Title { get; set; }
    public string Format { get; set; }
    public ObservableCollection<string> Formats { get; }

    // Movie-Specific Fields
    public string Director { get; set; }
    public int ReleaseYear { get; set; }

    // Book-Specific Fields
    public string Author { get; set; }
    public int Pages { get; set; }

    public bool IsMovie => _isMovie;
    public bool IsBook => !_isMovie;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddItemPageViewModel(bool isMovie, Action<object> onSave)
    {
        _isMovie = isMovie;
        _onSave = onSave;

        Formats = new ObservableCollection<string>(_isMovie
            ? new[] { "DVD", "Blu-ray", "Digital" }
            : new[] { "Paperback", "eBook" });

        SaveCommand = new Command(Save);
        CancelCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync());
    }

    private async void Save()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Format))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Title and format are required.", "OK");
            return;
        }

        if (_isMovie)
        {
            _onSave.Invoke(new Movie { Title = Title, Format = Format, Director = Director, ReleaseYear = ReleaseYear });
        }
        else
        {
            _onSave.Invoke(new Book { Title = Title, Format = Format, Author = Author, Pages = Pages });
        }

        await Application.Current.MainPage.Navigation.PopModalAsync();
    }
}
