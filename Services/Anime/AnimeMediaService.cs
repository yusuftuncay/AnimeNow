using AnimeNow.Models;
using System.Diagnostics;
using System.Text.Json;

namespace AnimeNow.Services.Anime
{
    public class AnimeMediaService
    {
        //
        private readonly HttpClient httpClient = new();
        private string hostname = AnimePreferencesService.Get("hostname");

        //
        public async Task<AniListAnimeMedia_Header> LoadHeaderAsync(string id)
        {
            try
            {
                string data = await LoadEpisodeAsync(id);

                AniListAnimeMedia AniListAnimeMedia = JsonSerializer.Deserialize<AniListAnimeMedia>(data)!;

                if (AniListAnimeMedia.Headers != null)
                    return AniListAnimeMedia.Headers;

                return new AniListAnimeMedia_Header();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AniListAnimeMedia_Source>> LoadSourceAsync(string id)
        {
            try
            {
                string data = await LoadEpisodeAsync(id);

                AniListAnimeMedia AniListAnimeMedia = JsonSerializer.Deserialize<AniListAnimeMedia>(data)!;

                if (AniListAnimeMedia.Sources != null)
                    return AniListAnimeMedia.Sources;

                return [];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> LoadEpisodeAsync(string id)
        {
            try
            {
                // kimetsu-no-yaiba-episode-20
                var response = await httpClient.GetAsync($"{hostname}/meta/anilist/watch/{id}");
                string data = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return data;

                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
