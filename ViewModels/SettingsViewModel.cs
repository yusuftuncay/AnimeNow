using AnimeNow.Models;
using AnimeNow.Services.AniList;
using AnimeNow.Services.Anime;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;

namespace AnimeNow.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        #region "Variables / Properties"
        [ObservableProperty]
        private ObservableCollection<string> providers = [];
        [ObservableProperty]
        private ObservableCollection<Hostname> hostnames = [];
        [ObservableProperty]
        private Hostname selectedHost = new();

        private readonly HttpClient httpClient = new();
        private readonly CancellationTokenSource CancellationTokenSource = new();

        // Version Number
        [ObservableProperty]
        string versionNumber = $"v{VersionTracking.Default.CurrentVersion}";

        [ObservableProperty]
        private bool hostnameEntryEnabled;
        [ObservableProperty]
        private string hostname;
        [ObservableProperty]
        private string provider;

        // WebView
        [ObservableProperty]
        private WebView webView;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsAniListLoginPageVisible))]
        private bool isSettingsPageVisible = true;
        public bool IsAniListLoginPageVisible => !IsSettingsPageVisible;

        // AniList
        [ObservableProperty]
        private string aniListButtonText;
        [ObservableProperty]
        private AniListProfile_Viewer aniListProfile = new();
        [ObservableProperty]
        private bool isLoggedIn = false;

        // RanOnce
        public static bool RanOnce = false;

        // Element IsEnabled
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;
        public bool IsNotBusy => !IsBusy;
        #endregion

        #region "Init"
        [RelayCommand]
        private async Task InitAsync()
        {
            if (IsBusy || RanOnce)
                return;

            IsBusy = true;

            try
            {
                // Initialize Providers
                Providers.Add("9Anime");
                Providers.Add("AnimeFox");
                Providers.Add("AnimePahe");
                Providers.Add("Enime");
                Providers.Add("GogoAnime");
                Providers.Add("Marin");
                Providers.Add("Zoro");

                // Initialize HostValues
                Hostnames.Add(new Hostname { Key = "Host 1", Value = "https://api.consumet.org" });
                Hostnames.Add(new Hostname { Key = "Host 2", Value = "https://c.delusionz.xyz" });
                Hostnames.Add(new Hostname { Key = "Host 3", Value = "https://march-api1.vercel.app" });
                Hostnames.Add(new Hostname { Key = "Host 4", Value = "https://a.tuncay.be" });
                Hostnames.Add(new Hostname { Key = "Custom", Value = "" });

                // SelectedHost for Picker
                SelectedHost = Hostnames.Where(x => x.Value == AnimePreferencesService.Get("hostname")).First();
                // Hostname Entry
                Hostname = Hostnames.Where(x => x.Value == AnimePreferencesService.Get("hostname")).Select(x => x.Value).First();
                // Provider
                Provider = AnimePreferencesService.Get("provider");
                // Token
                string token = AnimePreferencesService.Get("token");

                // Initialize AniListButton Text
                if (token.Length > 500)
                {
                    IsLoggedIn = true;
                    AniListProfile = await AniListService.FetchViewerData();
                    AniListButtonText = "Logout";
                }
                else
                {
                    AniListButtonText = "Login";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                RanOnce = true;
            }
        }
        [RelayCommand]
        private void InitWebView(WebView webView)
        {
            WebView = webView;
        }
        #endregion

        #region "Set Hostname"
        [RelayCommand]
        private async Task SelectedIndexChanged(Hostname hostname)
        {
            if (!RanOnce)
                return;

            if (hostname.Key == "Custom" || hostname.Key == "")
            {
                HostnameEntryEnabled = true;
                return;
            }
            else if (Hostnames.Where(x => x.Key == hostname.Key).Any())
            {
                HostnameEntryEnabled = false;
                await SetHostnameAsync(hostname);
            }
        }
        [RelayCommand]
        private async Task SetHostnameAsync(dynamic hostname)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (hostname is string)
                {
                    // Save
                    Hostname = hostname;
                    AnimePreferencesService.Set("hostname", hostname);

                    // Reload HomePage
                    AnimeHomeViewModel.RanOnce = false;
                    await Toast.Make("Success", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
                }
                else if (await CheckValidHostnameAsync(hostname.Value))
                {
                    // Save
                    Hostname = hostname.Value;
                    AnimePreferencesService.Set("hostname", hostname.Value);

                    // Reload HomePage
                    AnimeHomeViewModel.RanOnce = false;
                }
                else
                {
                    SelectedHost = Hostnames.Where(x => x.Value == AnimePreferencesService.Get("hostname")).First();
                    await Toast.Make("Failed", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                AnimePreferencesService.Set("hostname-changed", "true");
                IsBusy = false;
            }
        }
        public async Task<bool> CheckValidHostnameAsync(string url)
        {
            try
            {
                // Remove '/' from the end of the url
                if (url.EndsWith('/'))
                    url = url[..^1];

                HttpResponseMessage response = await httpClient.GetAsync(url);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region "Set Provider"
        [RelayCommand]
        private async Task SetProviderAsync(string provider)
        {
            // Set
            AnimePreferencesService.Set("provider", provider);

            // Save new Provider
            Provider = AnimePreferencesService.Get("provider");

            await Toast.Make($"Success", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
        }
        #endregion

        #region "AniList"
        [RelayCommand]
        private void AniListLogin(string buttonText)
        {
            if (buttonText == "Login")
                InitiateAniListLogin();
            else if (buttonText == "Logout")
                InitiateAniListLogout();
        }
        private void InitiateAniListLogin()
        {
            AniListButtonText = "Logout";

            // Show WebView & Go to AniList Website & Reload AniListCollectionPage
            WebView.Source = "https://anilist.co/api/v2/oauth/authorize?client_id=15206&response_type=token";
            IsSettingsPageVisible = false;
            AnimeAniListCollectionViewModel.RanOnce = false;
        }
        private async void InitiateAniListLogout()
        {
            // We reset every value, TRUSTING the user to press the "Logout" button manually
            AniListButtonText = "Login";
            AnimePreferencesService.Set("token", null);
            AnimePreferencesService.Set("loggedin-user-id", null);
            AniListProfile = null;
            IsLoggedIn = false;

            // Show WebView & Go to AniList Website & Reload AniListCollectionPage
            WebView.Source = "https://anilist.co";
            IsSettingsPageVisible = false;
            AnimeAniListCollectionViewModel.RanOnce = false;

            // Give Loading Time
            await Task.Delay(1500);

            // Scroll to Footer where "Logout" Button is Located
            await WebView.EvaluateJavaScriptAsync("document.querySelector('.links').scrollIntoView({ behavior: \"smooth\", inline: \"center\" });");

            // Press "Logout" Button
            //await WebView.EvaluateJavaScriptAsync("document.querySelector('.footer a[href=\"#\"]').click();");
        }
        [RelayCommand]
        private async Task WebViewNavigatedAsync()
        {
            if (IsSettingsPageVisible)
                return;

            // Token
            string javascriptToken = "document.getElementById('token').value;";
            var token = await WebView.EvaluateJavaScriptAsync(javascriptToken);
            if (token == null)
                return;

            // Join Now
            //string javascriptJoinNow = "document.querySelector('.landing .label')?.textContent.trim() === 'Join Now';";
            //string labelContent = await WebView.EvaluateJavaScriptAsync(javascriptJoinNow);
            //if (labelContent == "true") // The user logged out succesfully
            //    CloseWebView();

            // Token Check
            if (token.Length > 500)
                HandleSuccessfulLogin(token);
            else if (token == "undefined")
                HandleCancelledTask();
        }
        private async void HandleSuccessfulLogin(string token)
        {
            CloseWebView();

            AnimeAniListCollectionViewModel.RanOnce = false; // Reload AniListCollectionPage
            IsLoggedIn = true;
            AnimePreferencesService.Set("token", token);
            AniListProfile = await AniListService.FetchViewerData();
            AnimePreferencesService.Set("loggedin-user-id", AniListProfile.Id.ToString());
            AniListButtonText = "Logout";
        }
        private void HandleCancelledTask()
        {
            CloseWebView();

            AnimeAniListCollectionViewModel.RanOnce = false; // Reload AniListCollectionPage
            AnimePreferencesService.Set("token", null);
            AnimePreferencesService.Set("loggedin-user-id", null);
            AniListProfile = null;
            AniListButtonText = "Login";
        }
        [RelayCommand]
        private void CloseWebView()
        {
            WebView.Source = "https://anilist.co";
            IsSettingsPageVisible = true;
        }
        [RelayCommand]
        private void OpenProfile()
        {
            WebView.Source = AniListProfile.SiteUrl;
            IsSettingsPageVisible = false;
        }
        #endregion

        #region "Clear"
        [RelayCommand]
        private async Task ClearPreferencesAsync()
        {
            AnimePreferencesService.Clear();
            await Toast.Make("Success", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
        }
        [RelayCommand]
        private async Task ClearFavoritesAsync()
        {
            try
            {
                // Delete all files in the specified directory
                foreach (var file in Directory.GetFiles(AnimeDirectoryService.CollectionDirectory))
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await Toast.Make("Success", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
            }
        }
        [RelayCommand]
        private async Task ClearRecentWatchedAsync()
        {
            try
            {
                // Delete all files in the specified directory
                foreach (var file in Directory.GetFiles(AnimeDirectoryService.RecentWatchedDirectory))
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await Toast.Make("Success", ToastDuration.Short, 12).Show(CancellationTokenSource.Token);
            }
        }
        #endregion
    }
}
