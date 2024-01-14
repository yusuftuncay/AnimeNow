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
    public partial class AnimeHomeViewModel : ObservableObject
    {
        #region "Variables / Properties"
        [ObservableProperty]
        private ObservableCollection<AniListAnime_Result> trending = [];
        [ObservableProperty]
        private ObservableCollection<AniListAnime_Result> popular = [];
        [ObservableProperty]
        private ObservableCollection<AniListAnimeDetail_Episode> recentWatched = [];
        [ObservableProperty]
        private ObservableCollection<AniListAnimeRecentEpisodes_Result> recentEpisodes = [];

        private readonly AnimeHomeService homeService = new();
        private readonly CancellationTokenSource CancellationTokenSource = new();

        // Recent Watched
        [ObservableProperty]
        private int heightRecentWatchedTitle;
        [ObservableProperty]
        private int heightRecentWatchedContent;
        [ObservableProperty]
        private Thickness marginRecentWatchedTitle;
        [ObservableProperty]
        private Thickness marginRecentWatchedContent;

        // RanOnce
        public static bool RanOnce;

        // Element IsEnabled
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;
        public bool IsNotBusy => !IsBusy;
        #endregion

        #region "Init"
        [RelayCommand]
        private async Task GetAnimeAsync()
        {
            if (IsBusy || RanOnce)
                return;

            IsBusy = true;
            bool exceptionOccurred = false;

            try
            {
                // Initialize "RecentWatched" and "Collection" Directories
                AnimeDirectoryService.CreateDirectories();

                // If one request fails, it doesn't block the others
                try
                {
                    // Anilist Popular
                    var popular = await homeService.LoadAniListPopularAnimeAsync();
                    if (Popular.Count != 0)
                        Popular.Clear();
                    if (popular.Results != null) // Check
                        foreach (AniListAnime_Result row in popular.Results)
                            Popular.Add(row);
                }
                catch (Exception) { }
                try
                {
                    // Anilist Trending
                    var trending = await homeService.LoadAniListTrendingAnimeAsync();
                    if (Trending.Count != 0)
                        Trending.Clear();
                    if (trending.Results != null) // Check
                        foreach (AniListAnime_Result row in trending.Results)
                            Trending.Add(row);
                }
                catch (Exception) { }
                try
                {
                    // Anilist RecentEpisodes
                    var recentEpisodes = await homeService.LoadAniListAnimeRecentEpisodesEpisodesAsync();
                    if (RecentEpisodes.Count != 0)
                        RecentEpisodes.Clear();
                    if (recentEpisodes.Results != null) // Check
                        foreach (AniListAnimeRecentEpisodes_Result row in recentEpisodes.Results)
                            RecentEpisodes.Add(row);
                }
                catch (Exception) { }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                exceptionOccurred = true;
            }
            finally
            {
                if (!exceptionOccurred)
                    RanOnce = true;
                IsBusy = false;
            }
        }
        #endregion

        #region "Recent Watched"
        [RelayCommand]
        private void GetRecentWatched()
        {
            try
            {
                // Get all files with the .json extension in the directory
                string[] files = Directory.GetFiles(AnimeDirectoryService.RecentWatchedDirectory, "*.json");

                // Hide RecentWatched section if there are no files
                if (files.Length == 0)
                {
                    HeightRecentWatchedTitle = 0;
                    HeightRecentWatchedContent = 0;
                    MarginRecentWatchedTitle = 0;
                    MarginRecentWatchedContent = 0;
                    RecentWatched.Clear();
                    return;
                }
                else if (files.Length != 0)
                {
                    HeightRecentWatchedTitle = 20;
                    HeightRecentWatchedContent = 140;
                    MarginRecentWatchedTitle = new(24, 10);
                    MarginRecentWatchedContent = new(10, 0);
                    RecentWatched.Clear();

                    foreach (var filePath in files)
                    {
                        // Read the JSON data from the file
                        string json = File.ReadAllText(filePath);

                        // Deserialize the JSON data to AniListAnime_Result object
                        AniListAnimeDetail_Episode file = JsonConvert.DeserializeObject<AniListAnimeDetail_Episode>(json)!;

                        // Add the favorite to the collection
                        if (file != null)
                            RecentWatched.Add(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        [RelayCommand]
        private async Task DeleteEpisodeFromRecentWatched(string episodeId)
        {
            try
            {
                // Get info
                AniListAnimeDetail_Episode episode = RecentWatched.Where(x => x.Id == episodeId).FirstOrDefault()!;

                // Alert
                bool answer = await Shell.Current.CurrentPage.DisplayAlert("", "Delete this episode?", "Yes", "No");

                if (!answer)
                {
                    return;
                }
                else if (answer)
                {
                    string animePath = Path.Combine(AnimeDirectoryService.RecentWatchedDirectory, $"{episodeId}.json");

                    // Delete .json File
                    if (File.Exists(animePath))
                        File.Delete(animePath);

                    // Remove Episode from Collection
                    RecentWatched
                        .Remove(RecentWatched
                        .Where(x => x.Id == episodeId)
                        .FirstOrDefault());

                    // Update View
                    GetRecentWatched();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region "Navigation"
        [RelayCommand]
        private async Task GoToAnimeDetailPageAsync(AniListAnime_Result result)
        {
            await NavigationService.GoToAsync(nameof(AnimeDetailPage), true, "Result", result);
        }
        [RelayCommand]
        private async Task PlayRecentWatchedAnimeAsync(AniListAnimeDetail_Episode selectedEpisode)
        {
            await NavigationService.GoToAsync(nameof(AnimeMediaPage), false, "SelectedEpisode", selectedEpisode);
        }
        #endregion
    }
}
