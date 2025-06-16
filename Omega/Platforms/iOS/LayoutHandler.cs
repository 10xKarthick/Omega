using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Graphics;
using Omega.Controls.Layout;
using ILayoutHandler = Omega.Platforms.Handlers.ILayoutHandler;

namespace Omega.Platforms.iOS;

public class LayoutHandler : ViewHandler<LayoutBase, global::UIKit.UIView>, ILayoutHandler
{
    public LayoutHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
        : base(mapper ?? new PropertyMapper<LayoutBase>(), commandMapper)
    {
    }

    protected override global::UIKit.UIView CreatePlatformView()
    {
        var view = new global::UIKit.UIView();
        view.TranslatesAutoresizingMaskIntoConstraints = false;
        return view;
    }

    public Thickness GetSafeAreaInsets()
    {
        if (PlatformView?.Window?.SafeAreaInsets is global::UIKit.UIEdgeInsets insets)
        {
            return new Thickness(
                insets.Left,
                insets.Top,
                insets.Right,
                insets.Bottom
            );
        }

        return new Thickness(0);
    }
}