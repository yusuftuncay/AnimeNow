using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AnimeNow.Models
{
    public class AnimeHome
    {
        [JsonProperty("currentPage")]
        [JsonPropertyName("currentPage")]
        public int? CurrentPage { get; set; }

        [JsonProperty("hasNextPage")]
        [JsonPropertyName("hasNextPage")]
        public bool? HasNextPage { get; set; }

        [JsonProperty("results")]
        [JsonPropertyName("results")]
        public List<AnimeHome_Result> Results { get; set; }
    }

    public class AnimeHome_Result
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("malId")]
        [JsonPropertyName("malId")]
        public int? MalId { get; set; }

        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public AnimeHome_Title Title { get; set; }

        [JsonProperty("image")]
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("cover")]
        [JsonPropertyName("cover")]
        public string Cover { get; set; }

        [JsonProperty("rating")]
        [JsonPropertyName("rating")]
        public int? Rating { get; set; }

        [JsonProperty("releaseDate")]
        [JsonPropertyName("releaseDate")]
        public int? ReleaseDate { get; set; }

        [JsonProperty("color")]
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonProperty("genres")]
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("totalEpisodes")]
        [JsonPropertyName("totalEpisodes")]
        public int? TotalEpisodes { get; set; }

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class AnimeHome_Title
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

        [JsonProperty("userPreferred")]
        [JsonPropertyName("userPreferred")]
        public string UserPreferred { get; set; }
    }
}
