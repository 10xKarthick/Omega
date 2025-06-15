using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Omega.Controls.Layout;

/// <summary>
/// A widget that displays its children in a vertical array.
/// </summary>
public class Column : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the collection of child views in the column.
    /// </summary>
    public static readonly BindableProperty ChildrenProperty =
        BindableProperty.Create(
            nameof(Children),
            typeof(IList<View>),
            typeof(Column),
            new List<View>(),
            propertyChanged: (bindable, oldVal, newVal) => ((Column)bindable).UpdateColumn());

    /// <summary>
    /// Gets or sets the spacing between children in the column.
    /// </summary>
    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(Column),
            0.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Column)bindable).UpdateColumn());

    /// <summary>
    /// Gets or sets the horizontal options for the column.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Column),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Column)bindable).UpdateColumn());

    /// <summary>
    /// Gets or sets the vertical options for the column.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Column),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Column)bindable).UpdateColumn());

    /// <summary>
    /// Gets or sets the padding of the column.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Column),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Column)bindable).UpdateColumn());

    /// <summary>
    /// Gets or sets the margin of the column.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Column),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Column)bindable).UpdateColumn());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Column"/> class.
    /// </summary>
    public Column()
    {
        UpdateColumn();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the collection of child views in the column.
    /// </summary>
    public IList<View> Children
    {
        get => (IList<View>)GetValue(ChildrenProperty);
        set => SetValue(ChildrenProperty, value);
    }

    /// <summary>
    /// Gets or sets the spacing between children in the column.
    /// </summary>
    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the column.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the column.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the column.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the column.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateColumn()
    {
        var stack = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
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