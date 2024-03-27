using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AnimeNow.Models
{
    public class AnimeMedia
    {
        [JsonProperty("headers")]
        [JsonPropertyName("headers")]
        public AnimeMedia_Header Headers { get; set; }

        [JsonProperty("sources")]
        [JsonPropertyName("sources")]
        public List<AnimeMedia_Source> Sources { get; set; }

        [JsonProperty("download")]
        [JsonPropertyName("download")]
        public string Download { get; set; }
    }

    public class AnimeMedia_Header
    {
        [JsonProperty("Referer")]
        [JsonPropertyName("Referer")]
        public string Referer { get; set; }
    }

    public class AnimeMedia_Source
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
