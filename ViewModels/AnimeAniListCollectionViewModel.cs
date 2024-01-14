using AnimeNow.Models;
using AnimeNow.Services.AniList;
using AnimeNow.Services.General;
using AnimeNow.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AnimeNow.ViewModels
{
    public partial class AnimeAniListCollectionViewModel : ObservableObject
    {
        #region "Variables / Properties"
        [ObservableProperty]
        private ObservableCollection<AnimeAniListCollection_Entry> watching = [];
        [ObservableProperty]
        private ObservableCollection<AnimeAniListCollection_Entry> planning = [];
        [ObservableProperty]
        private ObservableCollection<AnimeAniListCollection_Entry> completed = [];

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

            try
            {
                // Initialize "RecentWatched" and "Collection" Directories
                AnimeAniListCollection_Root data =  await AniListService.FetchAniListCollectionData();

                if (data != null && data.MediaListCollection.Lists.Count != 0)
                {
                    Watching.Clear();
                    Completed.Clear();
                    Planning.Clear();

                    // Filter the lists with custom status
                    List<AnimeAniListCollection_List> currentList = data.MediaListCollection.Lists.Where(x => x.Status == "CURRENT").ToList();
                    List<AnimeAniListCollection_List> completedList = data.MediaListCollection.Lists.Where(x => x.Status == "COMPLETED").ToList();
                    List<AnimeAniListCollection_List> planningList = data.MediaListCollection.Lists.Where(x => x.Status == "PLANNING").ToList();

                    if (currentList.Count != 0)
                    {
                        // "CURRENT"
                        foreach (var list in currentList)
                        {
                            foreach (var entry in list.Entries)
                            {
                                Watching.Add(entry);
                            }
                        }
                    }

                    if (completedList.Count != 0)
                    {
                        // "COMPLETED"
                        foreach (var list in completedList)
                        {
                            foreach (var entry in list.Entries)
                            {
                                Completed.Add(entry);
                            }
                        }
                    }

                    if (planningList.Count != 0)
                    {
                        // "PLANNING"
                        foreach (var list in planningList)
                        {
                            foreach (var entry in list.Entries)
                            {
                                Planning.Add(entry);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                RanOnce = true;
                IsBusy = false;
            }
        }
        #endregion

        #region "Navigation"
        [RelayCommand]
        private async Task GoToAnimeDetailPageAsync(AniListAnime_Result result)
        {
            await NavigationService.GoToAsync(nameof(AnimeDetailPage), true, "Result", result);
        }
        #endregion
    }
}
