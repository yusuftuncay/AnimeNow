using AnimeNow.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace AnimeNow.Services.Anime
{
    public partial class AnimeRecentWatchedService : ObservableObject
    {
        #region "Save In RecentWatched"
        public static void SaveInRecentWatched(AnimeEpisode episode)
        {
            string animeTitle = episode.Id[..episode.Id.LastIndexOf("-episode-")]; // Example: shingeki-no-kyojin
            int episodeNumber = Convert.ToInt32(episode.Id[(episode.Id.LastIndexOf('-') + 1)..]); // Example: 22

            // Get existing Anime Files
            string existingFile = Directory.GetFiles(AnimeDirectoryService.RecentWatchedDirectory, $"{animeTitle}*.json").FirstOrDefault();

            // Check if there are any existing episodes, then save and exit
            if (existingFile == null)
            {
                SaveAnime(episode);
                return;
            }

            // Extract filename and episode number from the file name
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(existingFile);
            int savedEpisodeNumber = Convert.ToInt32(filenameWithoutExtension[(filenameWithoutExtension.LastIndexOf('-') + 1)..]);

            // Check if the latest saved episode is lower than the current episode
            if (savedEpisodeNumber < episodeNumber)
            {
                // Delete outdated file and save the anime
                File.Delete(existingFile);
                SaveAnime(episode);
            }
        }
        static void SaveAnime(AnimeEpisode episode)
        {
            string animePath = Path.Combine(AnimeDirectoryService.RecentWatchedDirectory, $"{episode.Id}.json");

            // Serialize the result object to JSON and save it to the directory
            string json = JsonConvert.SerializeObject(episode);
            File.WriteAllText(animePath, json);
        }
        #endregion

        #region "Check RecentWatched
        /// <summary>
        /// Checks if the current episode is in the "RecentWatched" Directory
        /// </summary>
        /// <param name="episode"></param>
        /// <returns>-1 if no episode found</returns>
        public static int CheckRecentWatched(string episode)
        {
            if (episode == null)
                return -1;

            string animeTitle = episode[..episode.LastIndexOf("-episode-")]; // Example: shingeki-no-kyojin

            // Get existing Episode File
            var existingFile = Directory.GetFiles(AnimeDirectoryService.RecentWatchedDirectory, $"{animeTitle}*.json").FirstOrDefault();

            if (existingFile == null)
                return -1;

            // Extract filename and episode number from the file name
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(existingFile);
            int savedEpisodeNumber = Convert.ToInt32(filenameWithoutExtension[(filenameWithoutExtension.LastIndexOf('-') + 1)..]);

            if (savedEpisodeNumber != 0)
                return savedEpisodeNumber;
            else
                return -1;
        }
        #endregion
    }
}
