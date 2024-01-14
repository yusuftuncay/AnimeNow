#if ANDROID
using Android.OS;
using Android.Views;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
#endif
using AnimeNow.ViewModels;
using AnimeNow.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace AnimeNow
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                // Font
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Afacad-Regular.ttf", "Afacad-Regular");
                })
                // Disable TabbedPage Swipe
#if ANDROID
                .ConfigureMauiHandlers
                (
                    handlers =>
                    {
                        handlers.AddHandler(typeof(Shell), typeof(CustomShellRenderer));
                    }
                )
#endif
                // VersionTracking
                .ConfigureEssentials(essentials =>
                {
                    essentials.UseVersionTracking();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            #region "Pages and Others"
            builder.Services.AddTransient<AnimeLocalCollectionPage>();
            builder.Services.AddTransient<AnimeLocalCollectionViewModel>();

            builder.Services.AddTransient<AnimeAniListCollectionPage>();
            builder.Services.AddTransient<AnimeAniListCollectionViewModel>();

            builder.Services.AddTransient<AnimeDetailPage>();
            builder.Services.AddTransient<AnimeDetailViewModel>();

            builder.Services.AddSingleton<AnimeHomePage>();
            builder.Services.AddSingleton<AnimeHomeViewModel>();

            builder.Services.AddTransient<AnimeMediaPage>();
            builder.Services.AddTransient<AnimeMediaViewModel>();

            builder.Services.AddSingleton<AnimeSearchPage>();
            builder.Services.AddSingleton<AnimeSearchViewModel>();

            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            #endregion

            return builder.Build();
        }
    }

#if ANDROID
    // Disable TabbedPage Swipe
    public class CustomShellRenderer : ShellRenderer
    {
        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            return new CustomShellSectionRenderer(this);
        }
    }
    public class CustomShellSectionRenderer(IShellContext shellContext) : ShellSectionRenderer(shellContext)
    {
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var result = base.OnCreateView(inflater, container, savedInstanceState);
            SetViewPager2UserInputEnabled(false);
            return result;
        }
        protected override void SetViewPager2UserInputEnabled(bool value)
        {
            base.SetViewPager2UserInputEnabled(false);
        }
    }
#endif
}
