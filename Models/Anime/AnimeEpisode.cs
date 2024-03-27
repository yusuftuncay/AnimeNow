using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AnimeNow.Models
{
    public class AnimeEpisode
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonProperty("image")]
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public int? Number { get; set; }

        [JsonProperty("createdAt")]
        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
