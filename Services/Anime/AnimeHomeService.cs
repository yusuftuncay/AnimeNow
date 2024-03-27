using AnimeNow.Models;
using System.Diagnostics;
using System.Net.Http.Json;

namespace AnimeNow.Services.Anime
{
    public class AnimeHomeService
    {
        //
        private readonly HttpClient httpClient = new();
        private string hostname = AnimePreferencesService.Get("hostname");

        //
        public async Task<AnimeHome> LoadAniListTrendingAnimeAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<AnimeHome>($"{hostname}/meta/anilist/trending?perPage=24");
                if (response != null)
                    return response;
                return new AnimeHome();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //
        public async Task<AnimeHome> LoadAniListPopularAnimeAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<AnimeHome>($"{hostname}/meta/anilist/popular?perPage=24");
                if (response != null)
                    return response;
                return new AnimeHome();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //
        public async Task<AnimeRecentEpisodes> LoadAniListAnimeRecentEpisodesEpisodesAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<AnimeRecentEpisodes>($"{hostname}/meta/anilist/recent-episodes?perPage=24");
                if (response != null)
                    return response;
                return new AnimeRecentEpisodes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
