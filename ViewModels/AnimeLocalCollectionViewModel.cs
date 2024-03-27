using AnimeNow.Models;
using AnimeNow.Services.Anime;
using AnimeNow.Services.General;
using AnimeNow.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AnimeNow.ViewModels
{
    public partial class AnimeLocalCollectionViewModel : ObservableObject
    {
        #region "Variables / Properties"
        [ObservableProperty]
        private ObservableCollection<AnimeHome_Result> favorites = [];

        // Element IsEnabled
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;
        public bool IsNotBusy => !IsBusy;
        #endregion

        #region "Init"
        [RelayCommand]
        private void GetAnime()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Favorites.Clear();

            try
            {
                // Get all files with the .json extension in the directory
                string[] files = Directory.GetFiles(AnimeDirectoryService.CollectionDirectory, "*.json");

                foreach (var filePath in files)
                {
                    try
                    {
                        // Read the JSON data from the file
                        string json = File.ReadAllText(filePath);

                        // Deserialize the JSON data to AniListAnime_Result object
                        AnimeHome_Result favorite = JsonConvert.DeserializeObject<AnimeHome_Result>(json)!;

                        // Add the favorite to the collection
                        if (favorite != null)
                        {
                            Favorites.Add(favorite);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
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
        private async Task GoToAnimeDetailPageAsync(AnimeHome_Result result)
        {
            await NavigationService.GoToAsync(nameof(AnimeDetailPage), true, "Result", result);
        }
        #endregion
    }
}
