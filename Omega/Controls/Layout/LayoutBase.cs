using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Platform;
using Omega.Platforms.Handlers;
using ILayoutHandler = Omega.Platforms.Handlers.ILayoutHandler;

namespace Omega.Controls.Layout;

/// <summary>
/// Base class for all layout controls in Omega
/// </summary>
public abstract class LayoutBase : Microsoft.Maui.Controls.Layout, ILayout, IDrawable
{
    public new static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(LayoutBase), new Thickness(0),
            propertyChanged: OnLayoutPropertyChanged);

    public new static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(LayoutBase), Colors.Transparent,
            propertyChanged: OnLayoutPropertyChanged);

    public new static readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(LayoutBase), true,
            propertyChanged: OnLayoutPropertyChanged);

    public new static readonly BindableProperty OpacityProperty =
        BindableProperty.Create(nameof(Opacity), typeof(double), typeof(LayoutBase), 1.0,
            propertyChanged: OnLayoutPropertyChanged);

    public new static readonly BindableProperty IsVisibleProperty =
        BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(LayoutBase), true,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty SafeAreaProperty =
        BindableProperty.Create(nameof(SafeArea), typeof(Thickness), typeof(LayoutBase), new Thickness(0),
            propertyChanged: OnLayoutPropertyChanged);

    private readonly List<IView> _children = [];

    /// <summary>
    /// Gets or sets the padding around the layout
    /// </summary>
    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the background color of the layout
    /// </summary>
    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the layout is enabled
    /// </summary>
    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the opacity of the layout
    /// </summary>
    public new double Opacity
    {
        get => (double)GetValue(OpacityProperty);
        set => SetValue(OpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets the visibility of the layout
    /// </summary>
    public new bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets the safe area insets for the layout
    /// </summary>
    public Thickness SafeArea
    {
        get => (Thickness)GetValue(SafeAreaProperty);
        set => SetValue(SafeAreaProperty, value);
    }

    /// <summary>
    /// Gets the collection of child elements in the layout
    /// </summary>
    public new IReadOnlyList<IView> Children => _children.AsReadOnly();

    protected LayoutBase()
    {
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is LayoutBase layout)
        {
            if (bindable is LayoutBase { } l && ReferenceEquals(bindable.GetValue(BackgroundColorProperty), newValue))
                l.BackgroundColor = (Color)newValue;
            layout.InvalidateMeasure();
        }
    }

    protected override void OnChildAdded(Element child)
    {
        base.OnChildAdded(child);
        if (child is IView view && IsChildVisible(view))
        {
            _children.Add(view);
            InvalidateMeasure();
        }
    }

    protected override void OnChildRemoved(Element child, int oldLogicalIndex)
    {
        base.OnChildRemoved(child, oldLogicalIndex);
        if (child is IView view)
        {
            _children.Remove(view);
            InvalidateMeasure();
        }
    }

    protected bool IsChildVisible(IView child) => child is not null && child.Visibility == Visibility.Visible;

    public void Invalidate() => InvalidateMeasure();

    public virtual void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // Base implementation does nothing
    }

    protected override Microsoft.Maui.Layouts.ILayoutManager CreateLayoutManager() => new LayoutManager(this);

    private class LayoutManager(LayoutBase layout) : Microsoft.Maui.Layouts.ILayoutManager
    {
        public Size Measure(double widthConstraint, double heightConstraint) =>
            layout.MeasureOverride(widthConstraint, heightConstraint);

        public Size ArrangeChildren(Rect bounds)
        {
            layout.ArrangeChildrenOverride(bounds);
            return new(bounds.Width, bounds.Height);
        }
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        if (Parent is not null && Application.Current is not null)
        {
            UpdateSafeArea();
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        }
        else if (Application.Current is not null)
        {
            Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
        }
    }

    private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e) => UpdateSafeArea();

    private void UpdateSafeArea()
    {
        if (Handler is Omega.Platforms.Handlers.ILayoutHandler layoutHandler)
        {
            SafeArea = layoutHandler.GetSafeAreaInsets();
        }
        else
        {
            // Fallback to display-based approximation if handler is not available
            var display = DeviceDisplay.Current.MainDisplayInfo;
            var width = display.Width / display.Density;
            var height = display.Height / display.Density;

            // Approximate safe area as 5% of screen dimensions
            SafeArea = new Thickness(
                width * 0.05,  // Left
                height * 0.05, // Top
                width * 0.05,  // Right
                height * 0.05  // Bottom
            );
        }
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        UpdateSafeArea();
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        if (Children.Count == 0)
            return new(widthConstraint, heightConstraint);

        var child = Children[0];
        var childSize = child.Measure(widthConstraint, heightConstraint);
        var padding = Padding;
        var safeArea = SafeArea;

        return new(
            childSize.Width + padding.Left + padding.Right + safeArea.Left + safeArea.Right,
            childSize.Height + padding.Top + padding.Bottom + safeArea.Top + safeArea.Bottom
        );
    }

    protected virtual void ArrangeChildrenOverride(Rect bounds)
    {
        if (Children.Count == 0)
            return;

        var child = Children[0];
        var padding = Padding;
        var safeArea = SafeArea;
        var childBounds = new Rect(
            bounds.X + padding.Left + safeArea.Left,
            bounds.Y + padding.Top + safeArea.Top,
            bounds.Width - padding.Left - padding.Right - safeArea.Left - safeArea.Right,
            bounds.Height - padding.Top - padding.Bottom - safeArea.Top - safeArea.Bottom
        );

        child.Arrange(childBounds);
    }
}