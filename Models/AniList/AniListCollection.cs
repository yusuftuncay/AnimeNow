namespace AnimeNow.Models
{
    public class AniListCollection_Root
    {
        public AniListCollection_MediaListCollection MediaListCollection { get; set; }
    }

    public class AniListCollection_MediaListCollection
    {
        public bool? HasNextChunk { get; set; }
        public AniListCollection_User User { get; set; }
        public List<AniListCollection_List> Lists { get; set; }
    }

    public class AniListCollection_User
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class AniListCollection_List
    {
        public string Status { get; set; }
        public List<AniListCollection_Entry> Entries { get; set; }
    }

    public class AniListCollection_Entry
    {
        public string Status { get; set; }
        public int? Progress { get; set; }
        public AniListCollection_Media Media { get; set; }
    }

    public class AniListCollection_Media
    {
        public int? Id { get; set; }
        public int? Episodes { get; set; }
        public AniListCollection_Title Title { get; set; }
        public string Description { get; set; }
        public AniListCollection_CoverImage CoverImage { get; set; }
    }

    public class AniListCollection_CoverImage
    {
        public string Medium { get; set; }
        public string Large { get; set; }
        public string ExtraLarge { get; set; }
    }

    public class AniListCollection_Title
    {
        public string Romaji { get; set; }
        public string English { get; set; }
        public string Native { get; set; }
        public string UserPreferred { get; set; }
    }
}