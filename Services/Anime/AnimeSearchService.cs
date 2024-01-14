using AnimeNow.Models;
using System.Diagnostics;
using System.Net.Http.Json;

namespace AnimeNow.Services.Anime
{
    public class AnimeSearchService
    {
        //
        private readonly HttpClient httpClient = new();
        private readonly string hostname = AnimePreferencesService.Get("hostname");

        //
        public async Task<AniListAnime> GetSearchResultAsync(string input)
        {
            try
            {
                if (input == null)
                    return new AniListAnime();

                return await httpClient.GetFromJsonAsync<AniListAnime>($"{hostname}/meta/anilist/advanced-search?query={input}&perPage=12") ?? new AniListAnime();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
