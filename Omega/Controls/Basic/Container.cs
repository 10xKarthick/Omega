using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Omega.Controls.Basic;

/// <summary>
/// A widget that combines common painting, positioning, and sizing widgets.
/// </summary>
public class Container : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the child view of the container.
    /// </summary>
    public static readonly BindableProperty ChildProperty =
        BindableProperty.Create(
            nameof(Child),
            typeof(View),
            typeof(Container),
            null,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the background color of the container.
    /// </summary>
    public static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(
            nameof(BackgroundColor),
            typeof(Color),
            typeof(Container),
            Colors.Transparent,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the border color of the container.
    /// </summary>
    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(Container),
            Colors.Transparent,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the border width of the container.
    /// </summary>
    public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(
            nameof(BorderWidth),
            typeof(double),
            typeof(Container),
            0.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the corner radius of the container.
    /// </summary>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(double),
            typeof(Container),
            0.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the padding of the container.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Container),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the margin of the container.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Container),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the horizontal options for the container.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Container),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the vertical options for the container.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Container),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the width of the container.
    /// </summary>
    public static readonly BindableProperty WidthProperty =
        BindableProperty.Create(
            nameof(Width),
            typeof(double),
            typeof(Container),
            -1.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the height of the container.
    /// </summary>
    public static readonly BindableProperty HeightProperty =
        BindableProperty.Create(
            nameof(Height),
            typeof(double),
            typeof(Container),
            -1.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    /// <summary>
    /// Gets or sets the elevation of the container.
    /// </summary>
    public static readonly BindableProperty ElevationProperty =
        BindableProperty.Create(
            nameof(Elevation),
            typeof(double),
            typeof(Container),
            0.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Container)bindable).UpdateContainer());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Container"/> class.
    /// </summary>
    public Container()
    {
        UpdateContainer();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the child view of the container.
    /// </summary>
    public View Child
    {
        get => (View)GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    /// <summary>
    /// Gets or sets the background color of the container.
    /// </summary>
    public Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the border color of the container.
    /// </summary>
    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the border width of the container.
    /// </summary>
    public double BorderWidth
    {
        get => (double)GetValue(BorderWidthProperty);
        set => SetValue(BorderWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the container.
    /// </summary>
    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the container.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the container.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the container.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the container.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the container.
    /// </summary>
    public double Width
    {
        get => (double)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the container.
    /// </summary>
    public double Height
    {
        get => (double)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the elevation of the container.
    /// </summary>
    public double Elevation
    {
        get => (double)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateContainer()
    {
        var frame = new Frame
        {
            BackgroundColor = BackgroundColor,
            BorderColor = BorderColor,
            CornerRadius = (float)CornerRadius,
            Padding = Padding,
            Margin = Margin,
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            WidthRequest = Width,
            HeightRequest = Height,
            HasShadow = Elevation > 0,
            Shadow = new Shadow
            {
                Brush = Colors.Black,
                Offset = new Point(0, Elevation),
                Radius = Elevation,
                Opacity = 0.25f
            }
        };

        if (BorderWidth > 0)
        {
            frame.BorderColor = BorderColor;
            frame.CornerRadius = (float)CornerRadius;
        }

        if (Child != null)
        {
            frame.Content = Child;
        }

        Content = frame;
    }

    #endregion
}