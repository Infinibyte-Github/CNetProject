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
}