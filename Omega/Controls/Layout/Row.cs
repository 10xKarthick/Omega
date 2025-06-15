using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Omega.Controls.Layout;

/// <summary>
/// A widget that displays its children in a horizontal array.
/// </summary>
public class Row : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the collection of child views in the row.
    /// </summary>
    public static readonly BindableProperty ChildrenProperty =
        BindableProperty.Create(
            nameof(Children),
            typeof(IList<View>),
            typeof(Row),
            new List<View>(),
            propertyChanged: (bindable, oldVal, newVal) => ((Row)bindable).UpdateRow());

    /// <summary>
    /// Gets or sets the spacing between children in the row.
    /// </summary>
    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(Row),
            0.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Row)bindable).UpdateRow());

    /// <summary>
    /// Gets or sets the horizontal options for the row.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Row),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Row)bindable).UpdateRow());

    /// <summary>
    /// Gets or sets the vertical options for the row.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Row),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Row)bindable).UpdateRow());

    /// <summary>
    /// Gets or sets the padding of the row.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Row),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Row)bindable).UpdateRow());

    /// <summary>
    /// Gets or sets the margin of the row.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Row),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Row)bindable).UpdateRow());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Row"/> class.
    /// </summary>
    public Row()
    {
        UpdateRow();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the collection of child views in the row.
    /// </summary>
    public IList<View> Children
    {
        get => (IList<View>)GetValue(ChildrenProperty);
        set => SetValue(ChildrenProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between children in the row.
    /// </summary>
    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the row.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the row.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the row.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the row.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateRow()
    {
        var stack = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = Spacing,
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            Padding = Padding,
            Margin = Margin
        };

        if (Children != null)
        {
            foreach (var child in Children.Where(c => c != null))
            {
                stack.Children.Add(child);
            }
        }

        Content = stack;
    }

    #endregion
} 