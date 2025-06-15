using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Omega.Controls.Basic;

/// <summary>
/// A widget that displays a graphical icon.
/// </summary>
public class Icon : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the source of the icon.
    /// </summary>
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(
            nameof(Source),
            typeof(string),
            typeof(Icon),
            string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => ((Icon)bindable).UpdateIcon());

    /// <summary>
    /// Gets or sets the color of the icon.
    /// </summary>
    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(
            nameof(Color),
            typeof(Color),
            typeof(Icon),
            Colors.Black,
            propertyChanged: (bindable, oldVal, newVal) => ((Icon)bindable).UpdateIcon());

    /// <summary>
    /// Gets or sets the size of the icon.
    /// </summary>
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(
            nameof(Size),
            typeof(double),
            typeof(Icon),
            24.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Icon)bindable).UpdateIcon());

    /// <summary>
    /// Gets or sets the horizontal options for the icon.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Icon),
            LayoutOptions.Center,
            propertyChanged: (bindable, oldVal, newVal) => ((Icon)bindable).UpdateIcon());

    /// <summary>
    /// Gets or sets the vertical options for the icon.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Icon),
            LayoutOptions.Center,
            propertyChanged: (bindable, oldVal, newVal) => ((Icon)bindable).UpdateIcon());

    /// <summary>
    /// Gets or sets the padding of the icon.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Icon),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Icon)bindable).UpdateIcon());

    /// <summary>
    /// Gets or sets the margin of the icon.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Icon),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Icon)bindable).UpdateIcon());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Icon"/> class.
    /// </summary>
    public Icon()
    {
        UpdateIcon();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the source of the icon.
    /// </summary>
    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the icon.
    /// </summary>
    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the size of the icon.
    /// </summary>
    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the icon.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the icon.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the icon.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the icon.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateIcon()
    {
        var image = new Image
        {
            Source = Source,
            TintColor = Color,
            WidthRequest = Size,
            HeightRequest = Size,
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            Padding = Padding,
            Margin = Margin,
            Aspect = Aspect.Fit
        };

        Content = image;
    }

    #endregion
} 