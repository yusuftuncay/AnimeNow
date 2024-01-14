namespace AnimeNow.Models
{
    public class AniListProfile
    {
        public AniListProfile_Viewer Viewer { get; set; }
    }

    public class AniListProfile_Viewer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public AniListProfile_Avatar Avatar { get; set; }
        public string BannerImage { get; set; }
        public string SiteUrl { get; set; }
        public AniListProfile_Options Options { get; set; }
    }

    public class AniListProfile_Avatar
    {
        public string Large { get; set; }
        public string Medium { get; set; }
    }

    public class AniListProfile_Options
    {
        public string TitleLanguage { get; set; }
        public string ProfileColor { get; set; }
    }
}
