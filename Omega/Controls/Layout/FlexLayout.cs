using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Omega.Controls.Layout;

/// <summary>
/// A flexible layout that arranges its children in a row or column with wrapping support
/// </summary>
public class FlexLayout : LayoutBase
{
    public static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(nameof(Direction), typeof(FlexDirection), typeof(FlexLayout), FlexDirection.Row,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty JustifyProperty =
        BindableProperty.Create(nameof(Justify), typeof(FlexJustify), typeof(FlexLayout), FlexJustify.Start,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty AlignProperty =
        BindableProperty.Create(nameof(Align), typeof(FlexAlign), typeof(FlexLayout), FlexAlign.Start,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty WrapProperty =
        BindableProperty.Create(nameof(Wrap), typeof(FlexWrap), typeof(FlexLayout), FlexWrap.NoWrap,
            propertyChanged: OnLayoutPropertyChanged);

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(double), typeof(FlexLayout), 0.0,
            propertyChanged: OnLayoutPropertyChanged);

    /// <summary>
    /// Gets or sets the direction in which child elements are arranged
    /// </summary>
    public FlexDirection Direction
    {
        get => (FlexDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    /// <summary>
    /// Gets or sets how child elements are justified along the main axis
    /// </summary>
    public FlexJustify Justify
    {
        get => (FlexJustify)GetValue(JustifyProperty);
        set => SetValue(JustifyProperty, value);
    }

    /// <summary>
    /// Gets or sets how child elements are aligned along the cross axis
    /// </summary>
    public FlexAlign Align
    {
        get => (FlexAlign)GetValue(AlignProperty);
        set => SetValue(AlignProperty, value);
    }

    /// <summary>
    /// Gets or sets whether child elements wrap to the next line/column
    /// </summary>
    public FlexWrap Wrap
    {
        get => (FlexWrap)GetValue(WrapProperty);
        set => SetValue(WrapProperty, value);
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
        if (bindable is FlexLayout layout)
        {
            layout.InvalidateMeasure();
        }
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        if (Children.Count == 0)
            return new(widthConstraint, heightConstraint);

        var isRow = Direction == FlexDirection.Row;
        var availableWidth = widthConstraint;
        var availableHeight = heightConstraint;
        var padding = Padding;

        // Adjust available space for padding
        availableWidth -= padding.Left + padding.Right;
        availableHeight -= padding.Top + padding.Bottom;

        var currentLine = new List<IView>();
        var lines = new List<List<IView>> { currentLine };
        var currentLineSize = new Size(0, 0);
        var totalSize = new Size(0, 0);

        foreach (var child in Children.Where(IsChildVisible))
        {
            var childSize = child.Measure(
                isRow ? availableWidth : availableWidth - currentLineSize.Width,
                isRow ? availableHeight - currentLineSize.Height : availableHeight
            );

            if (Wrap == FlexWrap.Wrap &&
                ((isRow && currentLineSize.Width + childSize.Width > availableWidth) ||
                 (!isRow && currentLineSize.Height + childSize.Height > availableHeight)))
            {
                // Move to next line
                if (isRow)
                {
                    totalSize.Width = Math.Max(totalSize.Width, currentLineSize.Width);
                    totalSize.Height += currentLineSize.Height;
                }
                else
                {
                    totalSize.Width += currentLineSize.Width;
                    totalSize.Height = Math.Max(totalSize.Height, currentLineSize.Height);
                }

                currentLine = [];
                lines.Add(currentLine);
                currentLineSize = new(0, 0);
            }

            currentLine.Add(child);
            if (isRow)
            {
                currentLineSize.Width += childSize.Width + (currentLine.Count > 1 ? Spacing : 0);
                currentLineSize.Height = Math.Max(currentLineSize.Height, childSize.Height);
            }
            else
            {
                currentLineSize.Width = Math.Max(currentLineSize.Width, childSize.Width);
                currentLineSize.Height += childSize.Height + (currentLine.Count > 1 ? Spacing : 0);
            }
        }

        // Add the last line
        if (isRow)
        {
            totalSize.Width = Math.Max(totalSize.Width, currentLineSize.Width);
            totalSize.Height += currentLineSize.Height;
        }
        else
        {
            totalSize.Width += currentLineSize.Width;
            totalSize.Height = Math.Max(totalSize.Height, currentLineSize.Height);
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

        var isRow = Direction == FlexDirection.Row;
        var padding = Padding;
        var availableBounds = new Rect(
            bounds.X + padding.Left,
            bounds.Y + padding.Top,
            bounds.Width - padding.Left - padding.Right,
            bounds.Height - padding.Top - padding.Bottom
        );

        var currentLine = new List<IView>();
        var lines = new List<List<IView>> { currentLine };
        var currentLineSize = new Size(0, 0);

        // First pass: measure and group into lines
        foreach (var child in Children.Where(IsChildVisible))
        {
            var childSize = child.Measure(
                isRow ? availableBounds.Width : availableBounds.Width - currentLineSize.Width,
                isRow ? availableBounds.Height - currentLineSize.Height : availableBounds.Height
            );

            if (Wrap == FlexWrap.Wrap &&
                ((isRow && currentLineSize.Width + childSize.Width > availableBounds.Width) ||
                 (!isRow && currentLineSize.Height + childSize.Height > availableBounds.Height)))
            {
                currentLine = [];
                lines.Add(currentLine);
                currentLineSize = new(0, 0);
            }

            currentLine.Add(child);
            if (isRow)
            {
                currentLineSize.Width += childSize.Width + (currentLine.Count > 1 ? Spacing : 0);
                currentLineSize.Height = Math.Max(currentLineSize.Height, childSize.Height);
            }
            else
            {
                currentLineSize.Width = Math.Max(currentLineSize.Width, childSize.Width);
                currentLineSize.Height += childSize.Height + (currentLine.Count > 1 ? Spacing : 0);
            }
        }

        // Second pass: arrange each line
        var currentY = availableBounds.Y;
        foreach (var line in lines)
        {
            if (line.Count == 0)
                continue;

            var lineSize = new Size(0, 0);
            foreach (var child in line)
            {
                var childSize = child.Measure(
                    isRow ? availableBounds.Width : availableBounds.Width - lineSize.Width,
                    isRow ? availableBounds.Height - lineSize.Height : availableBounds.Height
                );

                if (isRow)
                {
                    lineSize.Width += childSize.Width + (lineSize.Width > 0 ? Spacing : 0);
                    lineSize.Height = Math.Max(lineSize.Height, childSize.Height);
                }
                else
                {
                    lineSize.Width = Math.Max(lineSize.Width, childSize.Width);
                    lineSize.Height += childSize.Height + (lineSize.Height > 0 ? Spacing : 0);
                }
            }

            // Calculate line position based on justify and align
            var lineX = availableBounds.X;
            var lineY = currentY;
            var remainingSpace = isRow ?
                availableBounds.Width - lineSize.Width :
                availableBounds.Height - lineSize.Height;

            switch (Justify)
            {
                case FlexJustify.Center:
                    if (isRow)
                        lineX += remainingSpace / 2;
                    else
                        lineY += remainingSpace / 2;
                    break;
                case FlexJustify.End:
                    if (isRow)
                        lineX += remainingSpace;
                    else
                        lineY += remainingSpace;
                    break;
                case FlexJustify.SpaceBetween when line.Count > 1:
                    var spacing = remainingSpace / (line.Count - 1);
                    // Will be applied during child arrangement
                    break;
                case FlexJustify.SpaceAround:
                    var spaceAround = remainingSpace / (line.Count + 1);
                    if (isRow)
                        lineX += spaceAround;
                    else
                        lineY += spaceAround;
                    // Will be applied during child arrangement
                    break;
            }

            // Arrange children in the line
            var currentX = lineX;
            var currentChildY = lineY;
            var maxCrossSize = isRow ? lineSize.Height : lineSize.Width;

            foreach (var child in line)
            {
                var childSize = child.Measure(
                    isRow ? availableBounds.Width : availableBounds.Width - (currentX - lineX),
                    isRow ? availableBounds.Height - (currentChildY - lineY) : availableBounds.Height
                );

                var childX = currentX;
                var childY = currentChildY;

                // Apply cross-axis alignment
                switch (Align)
                {
                    case FlexAlign.Center:
                        if (isRow)
                            childY += (maxCrossSize - childSize.Height) / 2;
                        else
                            childX += (maxCrossSize - childSize.Width) / 2;
                        break;
                    case FlexAlign.End:
                        if (isRow)
                            childY += maxCrossSize - childSize.Height;
                        else
                            childX += maxCrossSize - childSize.Width;
                        break;
                }

                child.Arrange(new Rect(childX, childY, childSize.Width, childSize.Height));

                if (isRow)
                {
                    currentX += childSize.Width + Spacing;
                    if (Justify == FlexJustify.SpaceBetween && line.IndexOf(child) < line.Count - 1)
                        currentX += remainingSpace / (line.Count - 1);
                    else if (Justify == FlexJustify.SpaceAround)
                        currentX += remainingSpace / (line.Count + 1);
                }
                else
                {
                    currentChildY += childSize.Height + Spacing;
                    if (Justify == FlexJustify.SpaceBetween && line.IndexOf(child) < line.Count - 1)
                        currentChildY += remainingSpace / (line.Count - 1);
                    else if (Justify == FlexJustify.SpaceAround)
                        currentChildY += remainingSpace / (line.Count + 1);
                }
            }

            currentY += isRow ? lineSize.Height + Spacing : 0;
        }
    }

    protected override Microsoft.Maui.Layouts.ILayoutManager CreateLayoutManager() => new FlexLayoutManager(this);

    private class FlexLayoutManager(FlexLayout layout) : Microsoft.Maui.Layouts.ILayoutManager
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
/// Defines the direction in which child elements are arranged in a FlexLayout
/// </summary>
public enum FlexDirection
{
    /// <summary>
    /// Children are arranged in a row (horizontally)
    /// </summary>
    Row,

    /// <summary>
    /// Children are arranged in a column (vertically)
    /// </summary>
    Column
}

/// <summary>
/// Defines how child elements are justified along the main axis in a FlexLayout
/// </summary>
public enum FlexJustify
{
    /// <summary>
    /// Children are packed toward the start of the main axis
    /// </summary>
    Start,

    /// <summary>
    /// Children are packed toward the end of the main axis
    /// </summary>
    End,

    /// <summary>
    /// Children are centered along the main axis
    /// </summary>
    Center,

    /// <summary>
    /// Children are evenly distributed along the main axis, with equal spacing between them
    /// </summary>
    SpaceBetween,

    /// <summary>
    /// Children are evenly distributed along the main axis, with equal spacing around them
    /// </summary>
    SpaceAround
}

/// <summary>
/// Defines how child elements are aligned along the cross axis in a FlexLayout
/// </summary>
public enum FlexAlign
{
    /// <summary>
    /// Children are aligned to the start of the cross axis
    /// </summary>
    Start,

    /// <summary>
    /// Children are centered along the cross axis
    /// </summary>
    Center,

    /// <summary>
    /// Children are aligned to the end of the cross axis
    /// </summary>
    End
}

/// <summary>
/// Defines whether child elements wrap to the next line/column in a FlexLayout
/// </summary>
public enum FlexWrap
{
    /// <summary>
    /// Children do not wrap to the next line/column
    /// </summary>
    NoWrap,

    /// <summary>
    /// Children wrap to the next line/column when they reach the end of the main axis
    /// </summary>
    Wrap
}