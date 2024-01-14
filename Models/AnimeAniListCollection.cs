namespace AnimeNow.Models
{
    public class AnimeAniListCollection_Root
    {
        public AnimeAniListCollection_MediaListCollection MediaListCollection { get; set; }
    }

    public class AnimeAniListCollection_MediaListCollection
    {
        public bool? HasNextChunk { get; set; }
        public AnimeAniListCollection_User User { get; set; }
        public List<AnimeAniListCollection_List> Lists { get; set; }
    }

    public class AnimeAniListCollection_User
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class AnimeAniListCollection_List
    {
        public string Status { get; set; }
        public List<AnimeAniListCollection_Entry> Entries { get; set; }
    }

    public class AnimeAniListCollection_Entry
    {
        public string Status { get; set; }
        public int? Progress { get; set; }
        public AnimeAniListCollection_Media Media { get; set; }
    }

    public class AnimeAniListCollection_Media
    {
        public int? IdMal { get; set; }
        public int? Episodes { get; set; }
        public AnimeAniListCollection_Title Title { get; set; }
        public string Description { get; set; }
        public AnimeAniListCollection_CoverImage CoverImage { get; set; }
    }

    public class AnimeAniListCollection_CoverImage
    {
        public string Large { get; set; }
        public string Medium { get; set; }
    }

    public class AnimeAniListCollection_Title
    {
        public string Romaji { get; set; }
        public string English { get; set; }
        public string Native { get; set; }
        public string UserPreferred { get; set; }
    }
}