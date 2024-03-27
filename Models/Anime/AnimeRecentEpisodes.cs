using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AnimeNow.Models
{
    public class AnimeRecentEpisodes
    {
        [JsonProperty("currentPage")]
        [JsonPropertyName("currentPage")]
        public int? CurrentPage { get; set; }

        [JsonProperty("totalResults")]
        [JsonPropertyName("totalResults")]
        public int? TotalResults { get; set; }

        [JsonProperty("results")]
        [JsonPropertyName("results")]
        public List<AnimeRecentEpisodes_Result> Results { get; set; }
    }

    public class AnimeRecentEpisodes_Result
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public AnimeRecentEpisodes_Title Title { get; set; }

        [JsonProperty("image")]
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonProperty("episodeId")]
        [JsonPropertyName("episodeId")]
        public string EpisodeId { get; set; }

        [JsonProperty("episodeTitle")]
        [JsonPropertyName("episodeTitle")]
        public string EpisodeTitle { get; set; }

        [JsonProperty("episodeNumber")]
        [JsonPropertyName("episodeNumber")]
        public int? EpisodeNumber { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("malId")]
        [JsonPropertyName("malId")]
        public string MalId { get; set; }
    }

    public class AnimeRecentEpisodes_Title
    {
        [JsonProperty("romaji")]
        [JsonPropertyName("romaji")]
        public string Romaji { get; set; }

        [JsonProperty("english")]
        [JsonPropertyName("english")]
        public string English { get; set; }

        [JsonProperty("native")]
        [JsonPropertyName("native")]
        public string Native { get; set; }
    }
}
