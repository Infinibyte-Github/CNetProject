using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNetProject;

public partial class EditItemPage : ContentPage
{
    public EditItemPage(bool isMovie, Action<object> onSave)
    {
        InitializeComponent();
        BindingContext = new EditItemPageViewModel(isMovie, onSave);
    }
}