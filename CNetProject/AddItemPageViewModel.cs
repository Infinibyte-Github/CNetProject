namespace CNetProject;

using System.Windows.Input;

public class AddItemPageViewModel : BaseViewModel
{
    private readonly Action<string, string> _onSave;

    public string Title { get; set; }
    public string Format { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddItemPageViewModel(Action<string, string> onSave)
    {
        _onSave = onSave;
        SaveCommand = new Command(Save);
        CancelCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync());
    }

    private async void Save()
    {
        if (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Format))
        {
            _onSave.Invoke(Title, Format);
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Both fields are required.", "OK");
        }
    }
}
