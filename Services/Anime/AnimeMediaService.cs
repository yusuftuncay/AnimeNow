using AnimeNow.Models;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace AnimeNow.Services.Anime
{
    public class AnimeMediaService
    {
        //
        private readonly HttpClient httpClient = new();
        private string provider = AnimePreferencesService.Get("provider");
        private string hostname = AnimePreferencesService.Get("hostname");

        #region Episodes
        public async Task<AnimeMedia_Header> LoadHeaderAsync(string id)
        {
            try
            {
                string data = await LoadEpisodeAsync(id);

                AnimeMedia AniListAnimeMedia = JsonSerializer.Deserialize<AnimeMedia>(data)!;

                if (AniListAnimeMedia.Headers != null)
                    return AniListAnimeMedia.Headers;

                return new AnimeMedia_Header();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AnimeMedia_Source>> LoadSourceAsync(string id)
        {
            try
            {
                string data = await LoadEpisodeAsync(id);

                AnimeMedia AniListAnimeMedia = JsonSerializer.Deserialize<AnimeMedia>(data)!;

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
        #endregion

        #region Episode
        public async Task<List<AnimeEpisode>> LoadEpisodesAsync(string id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<AnimeEpisode>>($"{hostname}/meta/anilist/episodes/{id}?provider={provider.ToLower()}");

            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
