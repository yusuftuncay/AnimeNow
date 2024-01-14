namespace AnimeNow.Services.General
{
    public static class NavigationService
    {
        public static async Task GoToAsync(string page, bool animate, string queryProperty, object data)
        {
            await Shell.Current.GoToAsync(page, animate, new Dictionary<string, object>() { { queryProperty, data } });
        }
        public static async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}