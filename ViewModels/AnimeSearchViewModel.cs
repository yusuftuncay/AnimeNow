using AnimeNow.Models;
using AnimeNow.Services.Anime;
using AnimeNow.Services.General;
using AnimeNow.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AnimeNow.ViewModels
{
    public partial class AnimeSearchViewModel : ObservableObject
    {
        #region "Variables / Properties"
        [ObservableProperty]
        private ObservableCollection<AniListAnime_Result> searchResult = [];

        private readonly AnimeSearchService searchService = new();

        // Element IsEnabled
        [ObservableProperty]
        private bool isBusy;
        #endregion

        #region "Search"
        [RelayCommand]
        private async Task GetSearchResultAsync(string input)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            SearchResult.Clear();

            try
            {
                AniListAnime searchResult = await searchService.GetSearchResultAsync(input);

                if (searchResult.Results != null)
                    foreach (AniListAnime_Result item in searchResult.Results)
                        SearchResult.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region "Navigation"
        [RelayCommand]
        private static async Task AnimeDetailPageAsync(AniListAnime_Result result)
        {
            await NavigationService.GoToAsync(nameof(AnimeDetailPage), true, "Result", result);
        }
        #endregion
    }
}
