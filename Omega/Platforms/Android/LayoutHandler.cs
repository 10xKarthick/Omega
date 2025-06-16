using Android.App;
using Android.Views;
using AndroidX.Core.View;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Graphics;
using Omega.Controls.Layout;
using ILayoutHandler = Omega.Platforms.Handlers.ILayoutHandler;

namespace Omega.Platforms.Android;

public class LayoutHandler : ViewHandler<LayoutBase, global::Android.Views.View>, ILayoutHandler
{
    public LayoutHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
        : base(mapper ?? new PropertyMapper<LayoutBase>(), commandMapper)
    {
    }

    protected override global::Android.Views.View CreatePlatformView()
    {
        var view = new global::Android.Views.View(Context);
        ViewCompat.SetFitsSystemWindows(view, true);
        return view;
    }

    public Thickness GetSafeAreaInsets()
    {
        if (PlatformView is null || Context is not Activity activity || activity.Window?.DecorView is null)
            return new Thickness(0);

        var windowInsets = ViewCompat.GetRootWindowInsets(PlatformView);
        if (windowInsets is null)
            return new Thickness(0);

        var insets = windowInsets.GetInsets(WindowInsetsCompat.Type.SystemBars());
        return new Thickness(
            insets.Left / Context.Resources.DisplayMetrics.Density,
            insets.Top / Context.Resources.DisplayMetrics.Density,
            insets.Right / Context.Resources.DisplayMetrics.Density,
            insets.Bottom / Context.Resources.DisplayMetrics.Density
        );
    }
}