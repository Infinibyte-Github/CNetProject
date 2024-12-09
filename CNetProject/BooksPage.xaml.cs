using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNetProject;

public partial class BooksPage : ContentPage
{
    public BooksPage()
    {
        InitializeComponent();
        BindingContext = new BooksPageViewModel();
    }
    
    private void OnBookSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            // Reset the selected item to avoid repeated taps not firing
            ((ListView)sender).SelectedItem = null;
        }
    }
}