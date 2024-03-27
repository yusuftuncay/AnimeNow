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
        public async Task<AniListAnime> LoadAniListTrendingAnimeAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<AniListAnime>($"{hostname}/meta/anilist/trending?perPage=24");
                if (response != null)
                    return response;
                return new AniListAnime();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //
        public async Task<AniListAnime> LoadAniListPopularAnimeAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<AniListAnime>($"{hostname}/meta/anilist/popular?perPage=24");
                if (response != null)
                    return response;
                return new AniListAnime();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //
        public async Task<AniListAnimeRecentEpisodes> LoadAniListAnimeRecentEpisodesEpisodesAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<AniListAnimeRecentEpisodes>($"{hostname}/meta/anilist/recent-episodes?perPage=24");
                if (response != null)
                    return response;
                return new AniListAnimeRecentEpisodes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
