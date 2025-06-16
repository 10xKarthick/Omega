using Microsoft.Maui.Graphics;
using Omega.Controls.Layout;

namespace Omega.Controls.Basic;

public class Container : LayoutBase
{
    public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
        nameof(BackgroundColor),
        typeof(Color),
        typeof(Container),
        Colors.Transparent,
        propertyChanged: OnBackgroundColorChanged);

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
        nameof(BorderColor),
        typeof(Color),
        typeof(Container),
        Colors.Transparent,
        propertyChanged: OnBorderColorChanged);

    public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(
        nameof(BorderWidth),
        typeof(double),
        typeof(Container),
        0.0,
        propertyChanged: OnBorderWidthChanged);

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(Container),
        new CornerRadius(0),
        propertyChanged: OnCornerRadiusChanged);

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public double BorderWidth
    {
        get => (double)GetValue(BorderWidthProperty);
        set => SetValue(BorderWidthProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    private static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Container container)
        {
            container.Invalidate();
        }
    }

    private static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Container container)
        {
            container.Invalidate();
        }
    }

    private static void OnBorderWidthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Container container)
        {
            container.Invalidate();
        }
    }

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Container container)
        {
            container.Invalidate();
        }
    }

    public override void Draw(ICanvas canvas, RectF dirtyRect)
    {
        base.Draw(canvas, dirtyRect);

        // Draw background
        if (BackgroundColor != Colors.Transparent)
        {
            canvas.FillColor = BackgroundColor;
            canvas.FillRoundedRectangle(dirtyRect, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
        }

        // Draw border
        if (BorderColor != Colors.Transparent && BorderWidth > 0)
        {
            canvas.StrokeColor = BorderColor;
            canvas.StrokeSize = (float)BorderWidth;
            canvas.DrawRoundedRectangle(dirtyRect, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
        }
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        if (Children.Count == 0)
            return new Size(widthConstraint, heightConstraint);

        var child = Children[0];
        var childSize = child.Measure(widthConstraint, heightConstraint);
        var padding = Padding;
        var borderWidth = BorderWidth;

        return new Size(
            childSize.Width + padding.Left + padding.Right + borderWidth * 2,
            childSize.Height + padding.Top + padding.Bottom + borderWidth * 2
        );
    }

    protected override void ArrangeChildrenOverride(Rect bounds)
    {
        if (Children.Count == 0)
            return;

        var child = Children[0];
        var padding = Padding;
        var borderWidth = BorderWidth;
        var childBounds = new Rect(
            bounds.X + padding.Left + borderWidth,
            bounds.Y + padding.Top + borderWidth,
            bounds.Width - padding.Left - padding.Right - borderWidth * 2,
            bounds.Height - padding.Top - padding.Bottom - borderWidth * 2
        );

        child.Arrange(childBounds);
    }

    protected override Microsoft.Maui.Layouts.ILayoutManager CreateLayoutManager()
    {
        return new ContainerLayoutManager(this);
    }

    private class ContainerLayoutManager(Container container) : Microsoft.Maui.Layouts.ILayoutManager
    {
        public Size Measure(double widthConstraint, double heightConstraint)
        {
            return container.MeasureOverride(widthConstraint, heightConstraint);
        }

        public Size ArrangeChildren(Rect bounds)
        {
            container.ArrangeChildrenOverride(bounds);
            return new Size(bounds.Width, bounds.Height);
        }
    }
}