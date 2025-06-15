namespace Omega
{
    // All the code in this file is included in all platforms.
    public static class Omega
    {
        private static bool _isInitialized;

        public static MauiAppBuilder Initialize(this MauiAppBuilder builder)
        {
            if (_isInitialized)
            {
                return builder;
            }
            
            // Add any platform-specific services or configurations here
            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("MaterialIcons-Regular.ttf", "Filled");
                fonts.AddFont("MaterialIconsOutlined-Regular.otf", "Outlined");
                fonts.AddFont("MaterialIconsRound-Regular.ttf", "Round");   
            });
            
            // enable theming
            // ThemeManager initialize
            // ThemeManager.Initialize()
            
            _isInitialized = true;
            
            return builder; 
        }
    }
}
