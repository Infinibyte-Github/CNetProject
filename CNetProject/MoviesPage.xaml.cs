using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNetProject;

public partial class MoviesPage : ContentPage
{
    public MoviesPage()
    {
        InitializeComponent();
        BindingContext = new MoviesPageViewModel();
    }
    
    private void OnMovieSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            // Reset the selected item to avoid repeated taps not firing
            ((ListView)sender).SelectedItem = null;
        }
    }
}