using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Omega.Controls.Layout;

/// <summary>
/// A layout that arranges its children in a single line, either horizontally or vertically
/// </summary>
public class StackLayout : LayoutBase
{
    public static readonly BindableProperty OrientationProperty =
        BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(StackLayout), StackOrientation.Vertical,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty AlignmentProperty =
        BindableProperty.Create(nameof(Alignment), typeof(StackAlignment), typeof(StackLayout), StackAlignment.Start,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(double), typeof(StackLayout), 0.0,
            propertyChanged: OnLayoutPropertyChanged);

    /// <summary>
    /// Gets or sets the orientation of the stack layout
    /// </summary>
    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets how child elements are aligned within the stack layout
    /// </summary>
    public StackAlignment Alignment
    {
        get => (StackAlignment)GetValue(AlignmentProperty);
        set => SetValue(AlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between child elements
    /// </summary>
    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is StackLayout layout)
        {
            layout.InvalidateMeasure();
        }
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        if (Children.Count == 0)
            return new(widthConstraint, heightConstraint);

        var isVertical = Orientation == StackOrientation.Vertical;
        var availableWidth = widthConstraint;
        var availableHeight = heightConstraint;
        var padding = Padding;

        // Adjust available space for padding
        availableWidth -= padding.Left + padding.Right;
        availableHeight -= padding.Top + padding.Bottom;

        var totalSize = new Size(0, 0);
        var firstChild = true;

        foreach (var child in Children.Where(IsChildVisible))
        {
            var childSize = child.Measure(
                isVertical ? availableWidth : availableWidth - totalSize.Width,
                isVertical ? availableHeight - totalSize.Height : availableHeight
            );

            if (isVertical)
            {
                totalSize.Width = Math.Max(totalSize.Width, childSize.Width);
                totalSize.Height += childSize.Height + (firstChild ? 0 : Spacing);
            }
            else
            {
                totalSize.Width += childSize.Width + (firstChild ? 0 : Spacing);
                totalSize.Height = Math.Max(totalSize.Height, childSize.Height);
            }

            firstChild = false;
        }

        // Add padding
        totalSize.Width += padding.Left + padding.Right;
        totalSize.Height += padding.Top + padding.Bottom;

        return totalSize;
    }

    protected override void ArrangeChildrenOverride(Rect bounds)
    {
        if (Children.Count == 0)
            return;

        var isVertical = Orientation == StackOrientation.Vertical;
        var padding = Padding;
        var availableBounds = new Rect(
            bounds.X + padding.Left,
            bounds.Y + padding.Top,
            bounds.Width - padding.Left - padding.Right,
            bounds.Height - padding.Top - padding.Bottom
        );

        var currentX = availableBounds.X;
        var currentY = availableBounds.Y;
        var firstChild = true;

        // Sort children by ZIndex
        var sortedChildren = Children.Where(IsChildVisible).OrderBy(c => c.ZIndex);

        foreach (var child in sortedChildren)
        {
            var childSize = child.Measure(
                isVertical ? availableBounds.Width : availableBounds.Width - (currentX - availableBounds.X),
                isVertical ? availableBounds.Height - (currentY - availableBounds.Y) : availableBounds.Height
            );

            var childX = currentX;
            var childY = currentY;

            // Apply alignment
            if (isVertical)
            {
                switch (Alignment)
                {
                    case StackAlignment.Center:
                        childX += (availableBounds.Width - childSize.Width) / 2;
                        break;
                    case StackAlignment.End:
                        childX += availableBounds.Width - childSize.Width;
                        break;
                }
            }
            else
            {
                switch (Alignment)
                {
                    case StackAlignment.Center:
                        childY += (availableBounds.Height - childSize.Height) / 2;
                        break;
                    case StackAlignment.End:
                        childY += availableBounds.Height - childSize.Height;
                        break;
                }
            }

            child.Arrange(new Rect(childX, childY, childSize.Width, childSize.Height));

            if (isVertical)
                currentY += childSize.Height + (firstChild ? 0 : Spacing);
            else
                currentX += childSize.Width + (firstChild ? 0 : Spacing);

            firstChild = false;
        }
    }

    protected override Microsoft.Maui.Layouts.ILayoutManager CreateLayoutManager() => new StackLayoutManager(this);

    private class StackLayoutManager(StackLayout layout) : Microsoft.Maui.Layouts.ILayoutManager
    {
        public Size Measure(double widthConstraint, double heightConstraint) =>
            layout.MeasureOverride(widthConstraint, heightConstraint);

        public Size ArrangeChildren(Rect bounds)
        {
            layout.ArrangeChildrenOverride(bounds);
            return new(bounds.Width, bounds.Height);
        }
    }
}

/// <summary>
/// Defines the orientation of a StackLayout
/// </summary>
public enum StackOrientation
{
    /// <summary>
    /// Children are arranged vertically
    /// </summary>
    Vertical,

    /// <summary>
    /// Children are arranged horizontally
    /// </summary>
    Horizontal
}

/// <summary>
/// Defines how child elements are aligned within a StackLayout
/// </summary>
public enum StackAlignment
{
    /// <summary>
    /// Children are aligned to the start of the stack
    /// </summary>
    Start,

    /// <summary>
    /// Children are centered within the stack
    /// </summary>
    Center,

    /// <summary>
    /// Children are aligned to the end of the stack
    /// </summary>
    End
}