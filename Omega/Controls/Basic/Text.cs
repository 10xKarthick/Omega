using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Omega.Controls.Basic;

/// <summary>
/// A widget that displays a string of text with a single style.
/// </summary>
public class Text : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the text content to display.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(string),
            typeof(Text),
            string.Empty,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the color of the text.
    /// </summary>
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(Text),
            Colors.Black,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the font size of the text.
    /// </summary>
    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(Text),
            14.0,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the font family of the text.
    /// </summary>
    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(Text),
            null,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the font attributes of the text.
    /// </summary>
    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(
            nameof(FontAttributes),
            typeof(FontAttributes),
            typeof(Text),
            FontAttributes.None,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the horizontal text alignment.
    /// </summary>
    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(HorizontalTextAlignment),
            typeof(TextAlignment),
            typeof(Text),
            TextAlignment.Start,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the vertical text alignment.
    /// </summary>
    public static readonly BindableProperty VerticalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(VerticalTextAlignment),
            typeof(TextAlignment),
            typeof(Text),
            TextAlignment.Start,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the horizontal options for the text.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Text),
            LayoutOptions.Start,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the vertical options for the text.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Text),
            LayoutOptions.Start,
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the padding of the text.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Text),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    /// <summary>
    /// Gets or sets the margin of the text.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Text),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Text)bindable).UpdateText());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Text"/> class.
    /// </summary>
    public Text()
    {
        UpdateText();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the text content to display.
    /// </summary>
    public string Content
    {
        get => (string)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the text.
    /// </summary>
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the font size of the text.
    /// </summary>
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the font family of the text.
    /// </summary>
    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <summary>
    /// Gets or sets the font attributes of the text.
    /// </summary>
    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal text alignment.
    /// </summary>
    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical text alignment.
    /// </summary>
    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the text.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the text.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the text.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the text.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateText()
    {
        var label = new Label
        {
            Text = Content,
            TextColor = TextColor,
            FontSize = FontSize,
            FontFamily = FontFamily,
            FontAttributes = FontAttributes,
            HorizontalTextAlignment = HorizontalTextAlignment,
            VerticalTextAlignment = VerticalTextAlignment,
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            Padding = Padding,
            Margin = Margin
        };

        Content = label;
    }

    #endregion
} 