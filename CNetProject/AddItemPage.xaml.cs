using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNetProject;

public partial class AddItemPage : ContentPage
{
    public AddItemPage(Action<string, string> onSave)
    {
        InitializeComponent();
        BindingContext = new AddItemPageViewModel(onSave);
    }
}