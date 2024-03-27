using AnimeNow.Models;
using AnimeNow.Services.Anime;
using AnimeNow.Services.General;
using AnimeNow.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AnimeNow.ViewModels
{
    [QueryProperty(nameof(Result), "Result")]
    public partial class AnimeDetailViewModel : ObservableObject
    {
        #region "Variables / Properties"
        [ObservableProperty]
        public AnimeHome_Result result = new();
        [ObservableProperty]
        private ObservableCollection<AnimeEpisode> episodes = [];

        private readonly AnimeDetailService AnimeDetailService = new();
        private readonly CancellationTokenSource CancellationTokenSource = new();

        // Play
        [ObservableProperty]
        private AnimeEpisode selectedEpisode = new();
        [ObservableProperty]
        private string textPlayButton = "Play";

        [ObservableProperty]
        private bool isDescriptionVisible;
        [ObservableProperty]
        private string imageFavorite;

        // Element IsEnabled
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;
        public bool IsNotBusy => !IsBusy;
        [ObservableProperty]
        public bool isAnyEpisodesFound;
        #endregion

        #region "Init"
        [RelayCommand]
        private async Task InitEpisodes()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // Episodes
                if (Episodes.Count != 0)
                    return;

                Episodes = new(await AnimeDetailService.LoadEpisodesAsync(Result.Id));

                if (Episodes.Count == 0)
                {
                    await Toast.Make($"Error loading episodes", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
                    IsAnyEpisodesFound = false;
                }
                else
                {
                    IsAnyEpisodesFound = true;
                }

                // Play Button
                int episodeNumber = AnimeRecentWatchedService.CheckRecentWatched(Episodes.Select(x => x.Id).FirstOrDefault());
                if (episodeNumber != -1)
                    TextPlayButton = "Play EP: " + episodeNumber;
                else
                    TextPlayButton = "Play";
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
        [RelayCommand]
        private void Init(AnimeHome_Result result)
        {
            // Description Visibility
            if (PlatformService.GetPlatform() == 1)
                IsDescriptionVisible = true;
            else if (PlatformService.GetPlatform() == 2 || PlatformService.GetPlatform() == 3)
                IsDescriptionVisible = false;

            // Heart Icon / Collection
            if (File.Exists(Path.Combine(AnimeDirectoryService.CollectionDirectory, $"{result.Id}.json")))
                ImageFavorite = "heart.png";
            else
                ImageFavorite = "heart_outline.png";
        }
        #endregion

        #region "Play"
        [RelayCommand]
        public async Task PlayAsync()
        {
            // Play from RecentWatched
            if (TextPlayButton.Contains("EP"))
            {
                string pattern = @"\b(\d+)\b";
                Match match = Regex.Match(TextPlayButton, pattern);
                int episodeNumber = match.Success ? int.Parse(match.Groups[1].Value) : -1;

                SelectedEpisode = Episodes[episodeNumber - 1]; // Set the SelectedEpisode from RecentWatched
                AnimePreferencesService.Set("selected-anime-id", Result.Id); // Set Anime Id for later use
                AnimePreferencesService.Set("selected-anime-total-episodes", Result.TotalEpisodes.ToString()); // Set TotalEpisodes for later use
                await NavigationService.GoToAsync(nameof(AnimeMediaPage), false, "SelectedEpisode", SelectedEpisode);
            }
            // Play Normally
            else if (SelectedEpisode != null)
            {
                try
                {
                    AnimePreferencesService.Set("selected-anime-id", Result.Id); // Set Anime Id for later use
                    AnimePreferencesService.Set("selected-anime-total-episodes", Result.TotalEpisodes.ToString()); // Set TotalEpisodes for later use
                    await NavigationService.GoToAsync(nameof(AnimeMediaPage), false, "SelectedEpisode", SelectedEpisode);
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }
            }
            // No Episode Selected
            else if (SelectedEpisode == null)
            {
                await Toast.Make("Select an episode", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
            }
        }
        [RelayCommand]
        private void PickerSelectedIndexChanged()
        {
            TextPlayButton = "Play";
        }
        #endregion

        #region "Add To Collection"
        [RelayCommand]
        private async Task AddToCollection(AnimeHome_Result result)
        {
            string animeFile = Path.Combine(AnimeDirectoryService.CollectionDirectory, $"{result.Id}.json");

            // Check if this Anime is already favorite
            if (!File.Exists(animeFile))
            {
                try
                {
                    ImageFavorite = "heart.png";

                    // Serialize the result object to JSON and save it to the directory
                    string json = JsonConvert.SerializeObject(result);
                    File.WriteAllText(animeFile, json);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    await Toast.Make("Added to Collection", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
                }
            }
            else
            {
                try
                {
                    ImageFavorite = "heart_outline.png";
                    File.Delete(animeFile);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    await Toast.Make($"Removed From Collection", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
                }
            }
        }
        #endregion
    }
}
