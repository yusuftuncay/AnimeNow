using AnimeNow.ViewModels;

namespace AnimeNow.Views;

public partial class AnimeSearchPage : ContentPage
{
    public AnimeSearchPage(AnimeSearchViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}