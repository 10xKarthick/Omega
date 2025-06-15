using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Omega.Controls.Basic;

/// <summary>
/// A material design button that follows Flutter's button styling.
/// </summary>
public class Button : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the text displayed on the button.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(Button),
            string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the command to execute when the button is pressed.
    /// </summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(Button),
            null,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the parameter to pass to the command when the button is pressed.
    /// </summary>
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(Button),
            null,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the background color of the button.
    /// </summary>
    public static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(
            nameof(BackgroundColor),
            typeof(Color),
            typeof(Button),
            Colors.Transparent,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the text color of the button.
    /// </summary>
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(Button),
            Colors.Black,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the font size of the button text.
    /// </summary>
    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(Button),
            14.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the padding of the button.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Button),
            new Thickness(16, 8),
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the margin of the button.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Button),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the corner radius of the button.
    /// </summary>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(double),
            typeof(Button),
            4.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    /// <summary>
    /// Gets or sets the elevation of the button.
    /// </summary>
    public static readonly BindableProperty ElevationProperty =
        BindableProperty.Create(
            nameof(Elevation),
            typeof(double),
            typeof(Button),
            2.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Button)bindable).UpdateButton());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Button"/> class.
    /// </summary>
    public Button()
    {
        UpdateButton();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the text displayed on the button.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to execute when the button is pressed.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter to pass to the command when the button is pressed.
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the background color of the button.
    /// </summary>
    public Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the text color of the button.
    /// </summary>
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the button text.
    /// </summary>
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the button.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the button.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the button.
    /// </summary>
    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the elevation of the button.
    /// </summary>
    public double Elevation
    {
        get => (double)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateButton()
    {
        var frame = new Frame
        {
            BackgroundColor = BackgroundColor,
            CornerRadius = (float)CornerRadius,
            Padding = Padding,
            Margin = Margin,
            HasShadow = Elevation > 0,
            Shadow = new Shadow
            {
                Brush = Colors.Black,
                Offset = new Point(0, Elevation),
                Radius = Elevation,
                Opacity = 0.25f
            }
        };

        var label = new Label
        {
            Text = Text,
            TextColor = TextColor,
            FontSize = FontSize,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        frame.Content = label;

        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (sender, args) =>
        {
            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
        };

        frame.GestureRecognizers.Add(tapGestureRecognizer);

        Content = frame;
    }

    #endregion
} 