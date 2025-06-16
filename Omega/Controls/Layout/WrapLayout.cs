using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace Omega.Controls.Layout;

/// <summary>
/// A layout that automatically wraps children to the next line/column when they don't fit
/// </summary>
public class WrapLayout : LayoutBase
{
    public static readonly BindableProperty OrientationProperty =
        BindableProperty.Create(nameof(Orientation), typeof(WrapOrientation), typeof(WrapLayout), WrapOrientation.Horizontal,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(Size), typeof(WrapLayout), new Size(0, 0),
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty AlignmentProperty =
        BindableProperty.Create(nameof(Alignment), typeof(WrapAlignment), typeof(WrapLayout), WrapAlignment.Start,
            propertyChanged: OnLayoutPropertyChanged);

    /// <summary>
    /// Gets or sets the orientation of the wrap layout
    /// </summary>
    public WrapOrientation Orientation
    {
        get => (WrapOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between items
    /// </summary>
    public Size Spacing
    {
        get => (Size)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets how items are aligned within each line
    /// </summary>
    public WrapAlignment Alignment
    {
        get => (WrapAlignment)GetValue(AlignmentProperty);
        set => SetValue(AlignmentProperty, value);
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WrapLayout layout)
        {
            layout.InvalidateMeasure();
        }
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var availableWidth = widthConstraint;
        var availableHeight = heightConstraint;

        // Account for padding
        availableWidth -= Padding.Left + Padding.Right;
        availableHeight -= Padding.Top + Padding.Bottom;

        var isHorizontal = Orientation == WrapOrientation.Horizontal;
        var mainAxisAvailable = isHorizontal ? availableWidth : availableHeight;
        var crossAxisAvailable = isHorizontal ? availableHeight : availableWidth;

        var mainAxisSize = 0.0;
        var crossAxisSize = 0.0;
        var lineMainAxisSize = 0.0;
        var lineCrossAxisSize = 0.0;
        var lineCount = 0;
        var itemsInLine = 0;

        foreach (var child in Children)
        {
            if (!IsChildVisible(child))
                continue;

            var childSize = child.Measure(
                isHorizontal ? double.PositiveInfinity : availableWidth,
                isHorizontal ? availableHeight : double.PositiveInfinity
            );

            var childMainAxisSize = isHorizontal ? childSize.Width : childSize.Height;
            var childCrossAxisSize = isHorizontal ? childSize.Height : childSize.Width;

            // Check if we need to wrap
            if (lineMainAxisSize + childMainAxisSize > mainAxisAvailable && itemsInLine > 0)
            {
                // Move to next line
                mainAxisSize = Math.Max(mainAxisSize, lineMainAxisSize);
                crossAxisSize += lineCrossAxisSize + (lineCount > 0 ? Spacing.Height : 0);
                lineMainAxisSize = childMainAxisSize;
                lineCrossAxisSize = childCrossAxisSize;
                lineCount++;
                itemsInLine = 1;
            }
            else
            {
                lineMainAxisSize += childMainAxisSize + (itemsInLine > 0 ? Spacing.Width : 0);
                lineCrossAxisSize = Math.Max(lineCrossAxisSize, childCrossAxisSize);
                itemsInLine++;
            }
        }

        // Add the last line
        if (itemsInLine > 0)
        {
            mainAxisSize = Math.Max(mainAxisSize, lineMainAxisSize);
            crossAxisSize += lineCrossAxisSize + (lineCount > 0 ? Spacing.Height : 0);
        }

        // Add padding back
        mainAxisSize += isHorizontal ? Padding.Left + Padding.Right : Padding.Top + Padding.Bottom;
        crossAxisSize += isHorizontal ? Padding.Top + Padding.Bottom : Padding.Left + Padding.Right;

        return new Size(
            isHorizontal ? mainAxisSize : crossAxisSize,
            isHorizontal ? crossAxisSize : mainAxisSize
        );
    }

    protected override void ArrangeChildrenOverride(Rect bounds)
    {
        var isHorizontal = Orientation == WrapOrientation.Horizontal;
        var availableWidth = bounds.Width - Padding.Left - Padding.Right;
        var availableHeight = bounds.Height - Padding.Top - Padding.Bottom;

        var mainAxisAvailable = isHorizontal ? availableWidth : availableHeight;
        var crossAxisAvailable = isHorizontal ? availableHeight : availableWidth;

        var currentMainAxisPosition = isHorizontal ? Padding.Left : Padding.Top;
        var currentCrossAxisPosition = isHorizontal ? Padding.Top : Padding.Left;
        var lineMainAxisSize = 0.0;
        var lineCrossAxisSize = 0.0;
        var lineCount = 0;
        var lineStartIndex = 0;
        var itemsInLine = 0;

        for (int i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            if (!IsChildVisible(child))
                continue;

            var childSize = child.DesiredSize;
            var childMainAxisSize = isHorizontal ? childSize.Width : childSize.Height;
            var childCrossAxisSize = isHorizontal ? childSize.Height : childSize.Width;

            // Check if we need to wrap
            if (lineMainAxisSize + childMainAxisSize > mainAxisAvailable && itemsInLine > 0)
            {
                // Arrange the current line
                ArrangeLine(lineStartIndex, i - 1, lineMainAxisSize, lineCrossAxisSize,
                    currentMainAxisPosition, currentCrossAxisPosition, isHorizontal);

                // Move to next line
                currentCrossAxisPosition += lineCrossAxisSize + Spacing.Height;
                currentMainAxisPosition = isHorizontal ? Padding.Left : Padding.Top;
                lineMainAxisSize = childMainAxisSize;
                lineCrossAxisSize = childCrossAxisSize;
                lineCount++;
                lineStartIndex = i;
                itemsInLine = 1;
            }
            else
            {
                lineMainAxisSize += childMainAxisSize + (itemsInLine > 0 ? Spacing.Width : 0);
                lineCrossAxisSize = Math.Max(lineCrossAxisSize, childCrossAxisSize);
                itemsInLine++;
            }
        }

        // Arrange the last line
        if (itemsInLine > 0)
        {
            ArrangeLine(lineStartIndex, Children.Count - 1, lineMainAxisSize, lineCrossAxisSize,
                currentMainAxisPosition, currentCrossAxisPosition, isHorizontal);
        }
    }

    private void ArrangeLine(int startIndex, int endIndex, double lineMainAxisSize, double lineCrossAxisSize,
        double currentMainAxisPosition, double currentCrossAxisPosition, bool isHorizontal)
    {
        var mainAxisAvailable = isHorizontal ? Width - Padding.Left - Padding.Right : Height - Padding.Top - Padding.Bottom;
        var crossAxisAvailable = isHorizontal ? Height - Padding.Top - Padding.Bottom : Width - Padding.Left - Padding.Right;

        // Calculate main axis spacing based on Alignment
        var mainAxisSpacing = 0.0;
        var mainAxisStart = currentMainAxisPosition;

        switch (Alignment)
        {
            case WrapAlignment.SpaceBetween:
                if (endIndex > startIndex)
                {
                    mainAxisSpacing = (mainAxisAvailable - lineMainAxisSize) / (endIndex - startIndex);
                }
                break;
            case WrapAlignment.SpaceAround:
                mainAxisSpacing = (mainAxisAvailable - lineMainAxisSize) / (endIndex - startIndex + 1);
                mainAxisStart += mainAxisSpacing / 2;
                break;
            case WrapAlignment.SpaceEvenly:
                mainAxisSpacing = (mainAxisAvailable - lineMainAxisSize) / (endIndex - startIndex + 2);
                mainAxisStart += mainAxisSpacing;
                break;
            case WrapAlignment.Center:
                mainAxisStart += (mainAxisAvailable - lineMainAxisSize) / 2;
                break;
            case WrapAlignment.End:
                mainAxisStart += mainAxisAvailable - lineMainAxisSize;
                break;
        }

        // Arrange each child in the line
        var currentMainPos = mainAxisStart;
        for (int i = startIndex; i <= endIndex; i++)
        {
            var child = Children[i];
            if (!IsChildVisible(child))
                continue;

            var childSize = child.DesiredSize;
            var childMainAxisSize = isHorizontal ? childSize.Width : childSize.Height;
            var childCrossAxisSize = isHorizontal ? childSize.Height : childSize.Width;

            // Arrange the child
            var childBounds = isHorizontal
                ? new Rect(currentMainPos, currentCrossAxisPosition, childMainAxisSize, childCrossAxisSize)
                : new Rect(currentCrossAxisPosition, currentMainPos, childCrossAxisSize, childMainAxisSize);

            child.Arrange(childBounds);
            currentMainPos += childMainAxisSize + mainAxisSpacing;
        }
    }

    protected override Microsoft.Maui.Layouts.ILayoutManager CreateLayoutManager()
    {
        return new WrapLayoutManager(this);
    }

    private class WrapLayoutManager(WrapLayout layout) : Microsoft.Maui.Layouts.ILayoutManager
    {
        public Size Measure(double widthConstraint, double heightConstraint)
        {
            return layout.MeasureOverride(widthConstraint, heightConstraint);
        }

        public Size ArrangeChildren(Rect bounds)
        {
            layout.ArrangeChildrenOverride(bounds);
            return new Size(bounds.Width, bounds.Height);
        }
    }
}

/// <summary>
/// Defines the orientation of a WrapLayout
/// </summary>
public enum WrapOrientation
{
    /// <summary>
    /// Items are arranged horizontally and wrap to the next line
    /// </summary>
    Horizontal,

    /// <summary>
    /// Items are arranged vertically and wrap to the next column
    /// </summary>
    Vertical
}

/// <summary>
/// Defines how items are aligned within each line of a WrapLayout
/// </summary>
public enum WrapAlignment
{
    /// <summary>
    /// Items are packed toward the start of the line
    /// </summary>
    Start,

    /// <summary>
    /// Items are packed toward the end of the line
    /// </summary>
    End,

    /// <summary>
    /// Items are centered along the line
    /// </summary>
    Center,

    /// <summary>
    /// Items are evenly distributed along the line, with equal spacing between them
    /// </summary>
    SpaceBetween,

    /// <summary>
    /// Items are evenly distributed along the line, with equal spacing around them
    /// </summary>
    SpaceAround,

    /// <summary>
    /// Items are evenly distributed along the line, with equal spacing between and around them
    /// </summary>
    SpaceEvenly
}