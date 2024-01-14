namespace AnimeNow.Services.Anime
{
    public static class AnimePreferencesService
    {
        public static string Get(string key)
        {
            switch (key)
            {
                case "hostname":
                    return Preferences.Default.Get(key, "https://march-api1.vercel.app");
                case "provider":
                    return Preferences.Default.Get(key, "GogoAnime");
                case var k when k.Contains("episode"):
                    return Preferences.Default.Get(k, "00:00:00");
                case "volume":
                    return Preferences.Default.Get(key, 0.40.ToString());
                case "selected-anime-id":
                    return Preferences.Default.Get(key, "-1");

                default:
                    return Preferences.Default.Get(key, "");
            }
        }
        public static void Set(string key, string value)
        {
            Preferences.Default.Set(key, value);
        }
        public static void Clear()
        {
            Preferences.Default.Clear();
        }
    }
}
