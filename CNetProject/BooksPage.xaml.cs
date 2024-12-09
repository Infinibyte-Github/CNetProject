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
}