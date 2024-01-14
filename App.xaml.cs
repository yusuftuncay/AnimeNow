using AnimeNow.Services.General;

namespace AnimeNow
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            if (PlatformService.GetPlatform() == 1)
            {
                // Set a startup windowsize on Windows
                Window window = base.CreateWindow(activationState);

                window.Width = 620;
                window.Height = 990;

                // Move App to Center
                var disp = DeviceDisplay.Current.MainDisplayInfo;
                window.X = (disp.Width / disp.Density - window.Width) / 2;
                window.Y = (disp.Height / disp.Density - window.Height) / 2;

                return window;
            }
            else
            {
                Window window = base.CreateWindow(activationState);
                return window;
            }
        }
    }
}