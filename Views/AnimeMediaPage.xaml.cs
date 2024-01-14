using AnimeNow.ViewModels;

namespace AnimeNow.Views;

public partial class AnimeMediaPage : ContentPage
{
    public AnimeMediaPage(AnimeMediaViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}