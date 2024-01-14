namespace AnimeNow.Services.General
{
    public static class PlatformService
    {
        /// <summary>
        /// '1' for Windows<br/>
        /// '2' for iOS<br/>
        /// '3' for Android
        /// </summary>
        public static int GetPlatform()
        {
#if WINDOWS
            return 1;
#elif IOS
            return 2;
#elif ANDROID
            return 3;
#endif
#pragma warning disable CS0162
            return -1;
        }
    }
}
