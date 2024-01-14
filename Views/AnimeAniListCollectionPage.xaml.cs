using AnimeNow.ViewModels;

namespace AnimeNow.Views;

public partial class AnimeAniListCollectionPage : ContentPage
{
	public AnimeAniListCollectionPage(AnimeAniListCollectionViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}