using AnimeNow.ViewModels;

namespace AnimeNow.Views;

public partial class AnimeHomePage : ContentPage
{
    public AnimeHomePage(AnimeHomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}