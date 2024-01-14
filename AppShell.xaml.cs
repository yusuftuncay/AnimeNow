using AnimeNow.Views;

namespace AnimeNow
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("AnimeDetailPage", typeof(AnimeDetailPage));
            Routing.RegisterRoute("AnimeMediaPage", typeof(AnimeMediaPage));
        }
    }
}
