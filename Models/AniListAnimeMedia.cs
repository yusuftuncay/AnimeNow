using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AnimeNow.Models
{
    public class AniListAnimeMedia
    {
        [JsonProperty("headers")]
        [JsonPropertyName("headers")]
        public AniListAnimeMedia_Header Headers { get; set; }

        [JsonProperty("sources")]
        [JsonPropertyName("sources")]
        public List<AniListAnimeMedia_Source> Sources { get; set; }

        [JsonProperty("download")]
        [JsonPropertyName("download")]
        public string Download { get; set; }
    }

    public class AniListAnimeMedia_Header
    {
        [JsonProperty("Referer")]
        [JsonPropertyName("Referer")]
        public string Referer { get; set; }
    }

    public class AniListAnimeMedia_Source
    {
        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonProperty("isM3U8")]
        [JsonPropertyName("isM3U8")]
        public bool? IsM3U8 { get; set; }

        [JsonProperty("quality")]
        [JsonPropertyName("quality")]
        public string Quality { get; set; }
    }
}
