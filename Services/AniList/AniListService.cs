using AnimeNow.Models;
using AnimeNow.Services.Anime;
using AnimeNow.ViewModels;
using AnimeNow.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace AnimeNow.Services.AniList
{
    public partial class AniListService : ObservableObject
    {
        #region "Variables / Properties"
        private static readonly string endpoint = "https://graphql.anilist.co";

        // Element IsEnabled
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;
        public bool IsNotBusy => !IsBusy;
        #endregion

        #region "Update AniList Media Entry"
        public static async Task UpdateAniListMediaEntry(int? episodeNumber)
        {
            // Flag to refresh AnimeAniListCollectionPage
            AnimeAniListCollectionViewModel.RanOnce = false;

            // Variables
            string endpoint = "https://graphql.anilist.co";
            string token = AnimePreferencesService.Get("token");
            string selectedAnimeId = AnimePreferencesService.Get("selected-anime-id");
            string selectedAnimeTotalEpisodes = AnimePreferencesService.Get("selected-anime-total-episodes");
            string animeStatus;

            // Returns when user is not logged in
            if (token.Length < 500)
                return;

            // Set Anime Status
            if (episodeNumber >= Convert.ToInt32(selectedAnimeTotalEpisodes))
                animeStatus = "COMPLETED";
            else
                animeStatus = "CURRENT";

            // Initialize Client
            var graphQLHttpClient = new GraphQLHttpClient(endpoint, new NewtonsoftJsonSerializer());
            graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            // Mutation to add an anime to the user's anime list
            string mutation = $@"
                mutation {{
                    SaveMediaListEntry (mediaId: {selectedAnimeId}, status: {animeStatus}, progress: {episodeNumber}) {{
                        id
                    }}
                }}
            ";

            // GraphQL send mutation
            await graphQLHttpClient.SendMutationAsync<object>(
                new GraphQLRequest
                {
                    Query = mutation
                }
            );
        }
        #endregion

        #region FetchViewer Data
        public static async Task<AniListProfile_Viewer> FetchViewerData()
        {
            string endpoint = "https://graphql.anilist.co";
            string token = AnimePreferencesService.Get("token");

            if (token.Length < 500)
                return new AniListProfile_Viewer();

            var query = @"query FetchViewer {
                Viewer {
                    id
                    name
                    about
                    avatar {
                        large,
                        medium,
                    }
                    bannerImage,
                    siteUrl,
                    options {
                        titleLanguage
                        profileColor
                    }
                }
            }";

            var graphQLHttpClient = new GraphQLHttpClient(endpoint, new NewtonsoftJsonSerializer());
            graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var schema = await graphQLHttpClient.SendQueryAsync<AniListProfile>(new GraphQLRequest { Query = query });

            return schema.Data?.Viewer;
        }
        #endregion

        #region "Fetch AniList Collection Data"
        public static async Task<AnimeAniListCollection_Root> FetchAniListCollectionData()
        {
            string token = AnimePreferencesService.Get("token");

            if (token.Length < 500)
                return null;

            //6123985
            var query = $@"query MediaListCollection {{
                MediaListCollection(userId: {AnimePreferencesService.Get("loggedin-user-id")}, type: ANIME) {{
                    hasNextChunk
                    user {{
                        id
                        name
                    }}
                    lists {{
                        status
                        entries {{
                            status
                            progress
                            media {{
                                idMal
                                episodes
                                title {{
                                    romaji
                                    english
                                    native
                                    userPreferred
                                }}
                                description(asHtml: true)
                                coverImage {{
                                    medium
                                    large
                                }}
                            }}
                        }}
                    }}
                }}
            }}";

            var graphQLHttpClient = new GraphQLHttpClient(endpoint, new NewtonsoftJsonSerializer());
            graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var schema = await graphQLHttpClient.SendQueryAsync<AnimeAniListCollection_Root>(new GraphQLRequest { Query = query });

            return schema.Data;
        }
        #endregion
    }
}
