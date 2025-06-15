using Microsoft.Maui.Controls;

namespace Omega.Controls.Layout;

/// <summary>
/// A widget that attempts to size the child to a specific aspect ratio.
/// </summary>
public class AspectRatio : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the child view of the aspect ratio layout.
    /// </summary>
    public static readonly BindableProperty ChildProperty =
        BindableProperty.Create(
            nameof(Child),
            typeof(View),
            typeof(AspectRatio),
            null,
            propertyChanged: (bindable, oldVal, newVal) => ((AspectRatio)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the aspect ratio of the layout.
    /// </summary>
    public static readonly BindableProperty AspectRatioProperty =
        BindableProperty.Create(
            nameof(Ratio),
            typeof(double),
            typeof(AspectRatio),
            1.0,
            propertyChanged: (bindable, oldVal, newVal) => ((AspectRatio)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the horizontal options for the aspect ratio layout.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(AspectRatio),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((AspectRatio)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the vertical options for the aspect ratio layout.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(AspectRatio),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((AspectRatio)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the padding of the aspect ratio layout.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(AspectRatio),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((AspectRatio)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the margin of the aspect ratio layout.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(AspectRatio),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((AspectRatio)bindable).UpdateChild());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="AspectRatio"/> class.
    /// </summary>
    public AspectRatio()
    {
        UpdateChild();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the child view of the aspect ratio layout.
    /// </summary>
    public View Child
    {
        get => (View)GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    /// <summary>
    /// Gets or sets the aspect ratio of the layout.
    /// </summary>
    public double Ratio
    {
        get => (double)GetValue(AspectRatioProperty);
        set => SetValue(AspectRatioProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the aspect ratio layout.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the aspect ratio layout.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the aspect ratio layout.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the aspect ratio layout.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateChild()
    {
        if (Child == null)
        {
            Content = null;
            return;
        }

        // Create a container to hold the child with proper padding and margin
        var container = new Grid
        {
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            Padding = Padding,
            Margin = Margin,
            Children = { Child }
        };

        // Set the child's layout options to fill the container
        Child.HorizontalOptions = LayoutOptions.Fill;
        Child.VerticalOptions = LayoutOptions.Fill;

        // Calculate the aspect ratio constraint
        container.SizeChanged += (sender, args) =>
        {
            if (args.Width <= 0 || args.Height <= 0)
                return;

            var currentRatio = args.Width / args.Height;
            if (currentRatio > Ratio)
            {
                // Too wide, constrain width
                container.WidthRequest = args.Height * Ratio;
                container.HeightRequest = args.Height;
            }
            else
            {
                // Too tall, constrain height
                container.WidthRequest = args.Width;
                container.HeightRequest = args.Width / Ratio;
            }
        };

        Content = container;
    }

    #endregion
} 