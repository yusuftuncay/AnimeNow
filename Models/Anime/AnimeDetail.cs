using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AnimeNow.Models
{
    public class AnimeDetail
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public AnimeDetail_Title Title { get; set; }

        [JsonProperty("malId")]
        [JsonPropertyName("malId")]
        public int? MalId { get; set; }

        [JsonProperty("synonyms")]
        [JsonPropertyName("synonyms")]
        public List<string> Synonyms { get; set; }

        [JsonProperty("isLicensed")]
        [JsonPropertyName("isLicensed")]
        public bool? IsLicensed { get; set; }

        [JsonProperty("isAdult")]
        [JsonPropertyName("isAdult")]
        public bool? IsAdult { get; set; }

        [JsonProperty("countryOfOrigin")]
        [JsonPropertyName("countryOfOrigin")]
        public string CountryOfOrigin { get; set; }

        [JsonProperty("image")]
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonProperty("popularity")]
        [JsonPropertyName("popularity")]
        public int? Popularity { get; set; }

        [JsonProperty("color")]
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonProperty("cover")]
        [JsonPropertyName("cover")]
        public string Cover { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("releaseDate")]
        [JsonPropertyName("releaseDate")]
        public int? ReleaseDate { get; set; }

        [JsonProperty("startDate")]
        [JsonPropertyName("startDate")]
        public AnimeDetail_StartDate StartDate { get; set; }

        [JsonProperty("endDate")]
        [JsonPropertyName("endDate")]
        public AnimeDetail_EndDate EndDate { get; set; }

        [JsonProperty("totalEpisodes")]
        [JsonPropertyName("totalEpisodes")]
        public int? TotalEpisodes { get; set; }

        [JsonProperty("currentEpisode")]
        [JsonPropertyName("currentEpisode")]
        public int? CurrentEpisode { get; set; }

        [JsonProperty("rating")]
        [JsonPropertyName("rating")]
        public int? Rating { get; set; }

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        [JsonProperty("genres")]
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("season")]
        [JsonPropertyName("season")]
        public string Season { get; set; }

        [JsonProperty("studios")]
        [JsonPropertyName("studios")]
        public List<string> Studios { get; set; }

        [JsonProperty("subOrDub")]
        [JsonPropertyName("subOrDub")]
        public string SubOrDub { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("episodes")]
        [JsonPropertyName("episodes")]
        public List<AnimeDetail_Episode> Episodes { get; set; }
    }

    public class AnimeDetail_EndDate
    {
        [JsonProperty("year")]
        [JsonPropertyName("year")]
        public int? Year { get; set; }

        [JsonProperty("month")]
        [JsonPropertyName("month")]
        public int? Month { get; set; }

        [JsonProperty("day")]
        [JsonPropertyName("day")]
        public int? Day { get; set; }
    }

    public class AnimeDetail_Episode
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public int? Number { get; set; }

        [JsonProperty("image")]
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonProperty("airDate")]
        [JsonPropertyName("airDate")]
        public DateTime? AirDate { get; set; }
    }

    public class AnimeDetail_StartDate
    {
        [JsonProperty("year")]
        [JsonPropertyName("year")]
        public int? Year { get; set; }

        [JsonProperty("month")]
        [JsonPropertyName("month")]
        public int? Month { get; set; }

        [JsonProperty("day")]
        [JsonPropertyName("day")]
        public int? Day { get; set; }
    }

    public class AnimeDetail_Title
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
