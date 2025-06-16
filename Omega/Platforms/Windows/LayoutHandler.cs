using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Graphics;
using Omega.Controls.Layout;
using ILayoutHandler = Omega.Platforms.Handlers.ILayoutHandler;

namespace Omega.Platforms.Windows;

public class LayoutHandler : ViewHandler<LayoutBase, FrameworkElement>, ILayoutHandler
{
    public LayoutHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
        : base(mapper ?? new PropertyMapper<LayoutBase>(), commandMapper)
    {
    }

    protected override FrameworkElement CreatePlatformView()
    {
        var view = new FrameworkElement();
        return view;
    }

    public Thickness GetSafeAreaInsets()
    {
        if (PlatformView?.XamlRoot?.Content is not null)
        {
            var window = PlatformView.XamlRoot.HostWindow;
            if (window is not null)
            {
                var insets = window.AppWindow.TitleBar.ExtendsContentIntoTitleBar
                    ? new Thickness(0, window.AppWindow.TitleBar.Height, 0, 0)
                    : new Thickness(0);
                return insets;
            }
        }

        return new Thickness(0);
    }
}