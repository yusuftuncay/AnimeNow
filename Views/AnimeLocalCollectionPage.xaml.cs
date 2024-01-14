using AnimeNow.ViewModels;

namespace AnimeNow.Views;

public partial class AnimeLocalCollectionPage : ContentPage
{
    public AnimeLocalCollectionPage(AnimeLocalCollectionViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}