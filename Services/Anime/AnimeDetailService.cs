using AnimeNow.Models;
using System.Diagnostics;
using System.Text.Json;

namespace AnimeNow.Services.Anime
{
    public class AnimeDetailService
    {
        //
        private readonly HttpClient httpClient = new();
        private string provider = AnimePreferencesService.Get("provider");
        private string hostname = AnimePreferencesService.Get("hostname");

        //
        public async Task<List<AniListAnimeDetail_Episode>> GetEpisodeInfoAsync(string id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{hostname}/meta/anilist/info/{id}?provider={provider.ToLower()}");
                if (!response.IsSuccessStatusCode)
                    return [];

                var data = await response.Content.ReadAsStringAsync();

                // Parse the response body, only get the "episodes" property
                using JsonDocument doc = JsonDocument.Parse(data);
                return doc.RootElement.TryGetProperty("episodes", out JsonElement episodes)
                    ? JsonSerializer.Deserialize<List<AniListAnimeDetail_Episode>>(episodes.GetRawText()) ?? [] : [];

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}