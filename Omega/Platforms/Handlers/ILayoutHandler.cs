using Microsoft.Maui.Platform;
using Microsoft.Maui.Graphics;

namespace Omega.Platforms.Handlers;

public interface ILayoutHandler : IViewHandler
{
    /// <summary>
    /// Gets the safe area insets for the current platform
    /// </summary>
    /// <returns>The safe area insets as a Thickness</returns>
    Thickness GetSafeAreaInsets();
}