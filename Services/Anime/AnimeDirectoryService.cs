namespace AnimeNow.Services.Anime
{
    public static class AnimeDirectoryService
    {
        //
        public static readonly string CollectionDirectory = Path.Combine(FileSystem.AppDataDirectory, "Collection");
        public static readonly string RecentWatchedDirectory = Path.Combine(FileSystem.AppDataDirectory, "RecentWatched");

        //
        public static void CreateDirectories()
        {
            Directory.CreateDirectory(CollectionDirectory);
            Directory.CreateDirectory(RecentWatchedDirectory);
        }
    }
}
