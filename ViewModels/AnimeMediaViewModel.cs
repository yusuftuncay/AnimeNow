using AnimeNow.Models;
using AnimeNow.Services.AniList;
using AnimeNow.Services.Anime;
using AnimeNow.Services.General;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AnimeNow.ViewModels
{
    [QueryProperty(nameof(SelectedEpisode), "SelectedEpisode")]
    public partial class AnimeMediaViewModel : ObservableObject
    {
        #region "Variables / Properties"
        [ObservableProperty]
        private AnimeEpisode selectedEpisode = new();
        [ObservableProperty]
        private ObservableCollection<AnimeEpisode> episodes = [];
        [ObservableProperty]
        private AnimeMedia_Header headers = new();
        [ObservableProperty]
        private ObservableCollection<AnimeMedia_Source> sources = [];

        private readonly AnimeMediaService AnimeMediaService = new();

        [ObservableProperty]
        private string link = "";
        [ObservableProperty]
        private MediaElement mediaElement = new();
        [ObservableProperty]
        private bool isWatched92Percent;

        // Element IsEnabled
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;
        public bool IsNotBusy => !IsBusy;
        #endregion

        #region "Init"
        [RelayCommand]
        private async Task InitPlayerAsync(MediaElement m)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            MediaElement = m;

            try
            {
                // GetEpisodeInfo
                Headers = await AnimeMediaService.LoadHeaderAsync(SelectedEpisode.Id);
                Sources = new(await AnimeMediaService.LoadSourceAsync(SelectedEpisode.Id));
                Episodes = new(await AnimeMediaService.LoadEpisodesAsync(AnimePreferencesService.Get("selected-anime-id")));

                // Set Default Quality
                var sources = Sources.Where(x => x.Quality == "default").Select(x => x.Url).FirstOrDefault();
                if (sources != null)
                    Link = sources;

#if WINDOWS
                // Set Volume
                MediaElement.Volume = Convert.ToDouble(AnimePreferencesService.Get("volume"));
#endif

            } catch (Exception ex)
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
        private async Task SaveAndClean()
        {
            // Save In RecentWatched
            AnimeRecentWatchedService.SaveInRecentWatched(SelectedEpisode);

            // Only update AniList entry if user watched the entire episode
            if (IsWatched92Percent)
                await AniListService.UpdateAniListMediaEntry(SelectedEpisode.Number);

#if WINDOWS
            // Set Volume
            AnimePreferencesService.Set("volume", Convert.ToString(MediaElement.Volume));
#endif

            // Dispose
            MediaElement.Handler?.DisconnectHandler();
        }
        [RelayCommand]
        private static async Task GoBackAsync()
        {
            // Go Back
            await NavigationService.GoBack();
        }
        [RelayCommand]
        private async Task NextEpisodeAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Link = null;

            try
            {
                string animeTitle = SelectedEpisode.Id[..SelectedEpisode.Id.LastIndexOf("-episode-")]; // Example: shingeki-no-kyojin
                int episodeNumber = Convert.ToInt32(SelectedEpisode.Id[(SelectedEpisode.Id.LastIndexOf('-') + 1)..]) + 1; // Example: 22 + 1 (next episode)
                string nextEpisodeId = $"{animeTitle}-episode-{episodeNumber}";

                // Load Links
                Headers = await AnimeMediaService.LoadHeaderAsync(nextEpisodeId);
                Sources = new(await AnimeMediaService.LoadSourceAsync(nextEpisodeId));

                // Get Episode Info
                Episodes = new(await AnimeMediaService.LoadEpisodesAsync(AnimePreferencesService.Get("selected-anime-id")));
                SelectedEpisode = Episodes.Where(x => x.Id == nextEpisodeId).FirstOrDefault();

                // Set Quality
                var sources = Sources.Where(x => x.Quality == "default").Select(x => x.Url).FirstOrDefault();
                if (sources != null)
                    Link = sources;

                // Save In RecentWatched & Update episode number in AniList
                AnimeRecentWatchedService.SaveInRecentWatched(SelectedEpisode);
                await AniListService.UpdateAniListMediaEntry(SelectedEpisode.Number);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                MediaElement.Play();
            }
        }
        #endregion

        #region "MediaElement States"
        [RelayCommand]
        private async Task StateChangedAsync()
        {
            if (MediaElement.CurrentState == MediaElementState.Opening)
            {
                //
                await Task.Delay(200);
                switch (PlatformService.GetPlatform())
                {
                    case 1:
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            await MediaElement.SeekTo(TimeSpan.Parse(AnimePreferencesService.Get(SelectedEpisode.Id)));
                        });
                        break;
                    case 2:
                    case 3:
                        await MediaElement.SeekTo(TimeSpan.Parse(AnimePreferencesService.Get(SelectedEpisode.Id)));
                        break;
                }
            }
            else if (MediaElement.CurrentState == MediaElementState.Failed)
            {
                // Set New Quality if Playback Fails
                var source = Sources.Where(x => x.Quality == "backup").Select(x => x.Url).FirstOrDefault();
                if (source != null)
                    Link = source;
            }
        }
        [RelayCommand]
        private void PositionChanged()
        {
            // Don't show "Next Episode" button if User is watching last episode
            string selectedAnimeTotalEpisodes = AnimePreferencesService.Get("selected-anime-total-episodes");
            if (SelectedEpisode.Number >= Convert.ToInt32(selectedAnimeTotalEpisodes))
                return;

            // 92% of an episode shows a "Next Episode" button
            if (MediaElement.Position > MediaElement.Duration * 0.92)
                IsWatched92Percent = true;
            else
                IsWatched92Percent = false;

            // Save Position
            AnimePreferencesService.Set(
                    SelectedEpisode.Id, // Episode Id
                    MediaElement.Position.ToString());
        }
        #endregion
    }
}
