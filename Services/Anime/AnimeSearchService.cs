using AnimeNow.Models;
using System.Diagnostics;
using System.Net.Http.Json;

namespace AnimeNow.Services.Anime
{
    public class AnimeSearchService
    {
        //
        private readonly HttpClient httpClient = new();
        private string hostname = AnimePreferencesService.Get("hostname");

        //
        public async Task<AnimeHome> GetSearchResultAsync(string input)
        {
            try
            {
                if (input == null)
                    return new AnimeHome();

                return await httpClient.GetFromJsonAsync<AnimeHome>($"{hostname}/meta/anilist/advanced-search?query={input}&perPage=12") ?? new AnimeHome();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
