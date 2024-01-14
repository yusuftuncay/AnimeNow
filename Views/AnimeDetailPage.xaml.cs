using AnimeNow.ViewModels;

namespace AnimeNow.Views;

public partial class AnimeDetailPage : ContentPage
{
    public AnimeDetailPage(AnimeDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}