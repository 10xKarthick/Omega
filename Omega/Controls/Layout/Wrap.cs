using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Omega.Controls.Layout;

/// <summary>
/// A widget that displays its children in multiple horizontal or vertical runs.
/// </summary>
public class Wrap : ContentView
{
    private readonly Grid _grid;
    private readonly Dictionary<View, (int Row, int Column)> _childPositions = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Wrap"/> class.
    /// </summary>
    public Wrap()
    {
        _grid = new Grid
        {
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            Padding = Padding,
            Margin = Margin
        };
        Content = _grid;
        UpdateWrap();
    }

    #region Bindable Properties

    /// <summary>
    /// Gets or sets the children of the wrap layout.
    /// </summary>
    public static readonly BindableProperty ChildrenProperty =
        BindableProperty.Create(
            nameof(Children),
            typeof(IList<View>),
            typeof(Wrap),
            new List<View>(),
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the spacing between children in the same run.
    /// </summary>
    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(Wrap),
            0.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the spacing between runs.
    /// </summary>
    public static readonly BindableProperty RunSpacingProperty =
        BindableProperty.Create(
            nameof(RunSpacing),
            typeof(double),
            typeof(Wrap),
            0.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the alignment of children within each run.
    /// </summary>
    public static readonly BindableProperty AlignmentProperty =
        BindableProperty.Create(
            nameof(Alignment),
            typeof(WrapAlignment),
            typeof(Wrap),
            WrapAlignment.Start,
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the alignment of runs.
    /// </summary>
    public static readonly BindableProperty RunAlignmentProperty =
        BindableProperty.Create(
            nameof(RunAlignment),
            typeof(WrapAlignment),
            typeof(Wrap),
            WrapAlignment.Start,
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the direction of the wrap layout.
    /// </summary>
    public static readonly BindableProperty DirectionProperty =
        BindableProperty.Create(
            nameof(Direction),
            typeof(WrapDirection),
            typeof(Wrap),
            WrapDirection.Horizontal,
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the horizontal options for the wrap layout.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Wrap),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the vertical options for the wrap layout.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Wrap),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the padding of the wrap layout.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Wrap),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    /// <summary>
    /// Gets or sets the margin of the wrap layout.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Wrap),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Wrap)bindable).UpdateWrap());

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the children of the wrap layout.
    /// </summary>
    public IList<View> Children
    {
        get => (IList<View>)GetValue(ChildrenProperty);
        set => SetValue(ChildrenProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between children in the same run.
    /// </summary>
    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between runs.
    /// </summary>
    public double RunSpacing
    {
        get => (double)GetValue(RunSpacingProperty);
        set => SetValue(RunSpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the alignment of children within each run.
    /// </summary>
    public WrapAlignment Alignment
    {
        get => (WrapAlignment)GetValue(AlignmentProperty);
        set => SetValue(AlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the alignment of runs.
    /// </summary>
    public WrapAlignment RunAlignment
    {
        get => (WrapAlignment)GetValue(RunAlignmentProperty);
        set => SetValue(RunAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the direction of the wrap layout.
    /// </summary>
    public WrapDirection Direction
    {
        get => (WrapDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the wrap layout.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the wrap layout.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the wrap layout.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the wrap layout.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateWrap()
    {
        if (_grid == null) return;

        _grid.Children.Clear();
        _grid.RowDefinitions.Clear();
        _grid.ColumnDefinitions.Clear();
        _childPositions.Clear();

        if (Children == null || !Children.Any()) return;

        var isHorizontal = Direction == WrapDirection.Horizontal;
        var availableWidth = _grid.Width;
        var availableHeight = _grid.Height;

        if (availableWidth <= 0 || availableHeight <= 0) return;

        var currentRow = 0;
        var currentColumn = 0;
        var currentLineWidth = 0.0;
        var currentLineHeight = 0.0;
        var maxLineHeight = 0.0;

        // First pass: Calculate positions and create grid structure
        foreach (var child in Children)
        {
            var childSize = child.Measure(availableWidth, availableHeight);
            var childWidth = childSize.Width;
            var childHeight = childSize.Height;

            if (isHorizontal)
            {
                if (currentLineWidth + childWidth > availableWidth && currentColumn > 0)
                {
                    // Move to next row
                    currentRow++;
                    currentColumn = 0;
                    currentLineWidth = childWidth;
                    maxLineHeight = Math.Max(maxLineHeight, currentLineHeight);
                    currentLineHeight = childHeight;
                }
                else
                {
                    currentLineWidth += childWidth + (currentColumn > 0 ? Spacing : 0);
                    currentLineHeight = Math.Max(currentLineHeight, childHeight);
                }

                _childPositions[child] = (currentRow, currentColumn);
                currentColumn++;
            }
            else
            {
                if (currentLineHeight + childHeight > availableHeight && currentRow > 0)
                {
                    // Move to next column
                    currentColumn++;
                    currentRow = 0;
                    currentLineHeight = childHeight;
                    maxLineHeight = Math.Max(maxLineHeight, currentLineWidth);
                    currentLineWidth = childWidth;
                }
                else
                {
                    currentLineHeight += childHeight + (currentRow > 0 ? Spacing : 0);
                    currentLineWidth = Math.Max(currentLineWidth, childWidth);
                }

                _childPositions[child] = (currentRow, currentColumn);
                currentRow++;
            }
        }

        // Create grid structure
        var maxRow = _childPositions.Values.Max(p => p.Row) + 1;
        var maxColumn = _childPositions.Values.Max(p => p.Column) + 1;

        for (int i = 0; i < maxRow; i++)
        {
            _grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        for (int i = 0; i < maxColumn; i++)
        {
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        }

        // Second pass: Add children to grid with proper alignment
        foreach (var child in Children)
        {
            if (!_childPositions.TryGetValue(child, out var position)) continue;

            var (row, column) = position;
            Grid.SetRow(child, row);
            Grid.SetColumn(child, column);

            // Apply alignment
            child.HorizontalOptions = Alignment switch
            {
                WrapAlignment.Start => LayoutOptions.Start,
                WrapAlignment.End => LayoutOptions.End,
                WrapAlignment.Center => LayoutOptions.Center,
                WrapAlignment.SpaceBetween => LayoutOptions.Fill,
                WrapAlignment.SpaceAround => LayoutOptions.Fill,
                WrapAlignment.SpaceEvenly => LayoutOptions.Fill,
                _ => LayoutOptions.Start
            };

            child.VerticalOptions = RunAlignment switch
            {
                WrapAlignment.Start => LayoutOptions.Start,
                WrapAlignment.End => LayoutOptions.End,
                WrapAlignment.Center => LayoutOptions.Center,
                WrapAlignment.SpaceBetween => LayoutOptions.Fill,
                WrapAlignment.SpaceAround => LayoutOptions.Fill,
                WrapAlignment.SpaceEvenly => LayoutOptions.Fill,
                _ => LayoutOptions.Start
            };

            _grid.Children.Add(child);
        }

        // Apply spacing
        for (int i = 0; i < maxRow; i++)
        {
            _grid.RowSpacing = RunSpacing;
        }

        for (int i = 0; i < maxColumn; i++)
        {
            _grid.ColumnSpacing = Spacing;
        }

        _grid.HorizontalOptions = HorizontalOptions;
        _grid.VerticalOptions = VerticalOptions;
        _grid.Padding = Padding;
        _grid.Margin = Margin;
    }

    #endregion
}

/// <summary>
/// Defines the alignment of children within a wrap layout.
/// </summary>
public enum WrapAlignment
{
    /// <summary>
    /// Aligns children to the start of the container.
    /// </summary>
    Start,

    /// <summary>
    /// Aligns children to the end of the container.
    /// </summary>
    End,

    /// <summary>
    /// Centers children in the container.
    /// </summary>
    Center,

    /// <summary>
    /// Places free space evenly between the children.
    /// </summary>
    SpaceBetween,

    /// <summary>
    /// Places free space evenly between the children as well as half of that space before and after the first and last child.
    /// </summary>
    SpaceAround,

    /// <summary>
    /// Places free space evenly between the children as well as before and after the first and last child.
    /// </summary>
    SpaceEvenly
}

/// <summary>
/// Defines the direction of a wrap layout.
/// </summary>
public enum WrapDirection
{
    /// <summary>
    /// Children are arranged horizontally and wrap to the next line when they reach the end.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Children are arranged vertically and wrap to the next column when they reach the bottom.
    /// </summary>
    Vertical
} 