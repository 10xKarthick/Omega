using Microsoft.Maui.Controls;

namespace Omega.Controls.Layout;

/// <summary>
/// A widget that controls how a child of a Row, Column, or Flex flexes.
/// </summary>
public class Flexible : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the child view of the flexible layout.
    /// </summary>
    public static readonly BindableProperty ChildProperty =
        BindableProperty.Create(
            nameof(Child),
            typeof(View),
            typeof(Flexible),
            null,
            propertyChanged: (bindable, oldVal, newVal) => ((Flexible)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the flex factor of the flexible layout.
    /// </summary>
    public static readonly BindableProperty FlexProperty =
        BindableProperty.Create(
            nameof(Flex),
            typeof(int),
            typeof(Flexible),
            1,
            propertyChanged: (bindable, oldVal, newVal) => ((Flexible)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the fit behavior of the flexible layout.
    /// </summary>
    public static readonly BindableProperty FitProperty =
        BindableProperty.Create(
            nameof(Fit),
            typeof(FlexFit),
            typeof(Flexible),
            FlexFit.Loose,
            propertyChanged: (bindable, oldVal, newVal) => ((Flexible)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the horizontal options for the flexible layout.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Flexible),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Flexible)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the vertical options for the flexible layout.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Flexible),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Flexible)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the padding of the flexible layout.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Flexible),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Flexible)bindable).UpdateChild());

    /// <summary>
    /// Gets or sets the margin of the flexible layout.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Flexible),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Flexible)bindable).UpdateChild());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Flexible"/> class.
    /// </summary>
    public Flexible()
    {
        UpdateChild();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the child view of the flexible layout.
    /// </summary>
    public View Child
    {
        get => (View)GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    /// <summary>
    /// Gets or sets the flex factor of the flexible layout.
    /// </summary>
    public int Flex
    {
        get => (int)GetValue(FlexProperty);
        set => SetValue(FlexProperty, value);
    }

    /// <summary>
    /// Gets or sets the fit behavior of the flexible layout.
    /// </summary>
    public FlexFit Fit
    {
        get => (FlexFit)GetValue(FitProperty);
        set => SetValue(FitProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the flexible layout.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the flexible layout.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the flexible layout.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the flexible layout.
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

        // Set the child's layout options based on the Fit property
        Child.HorizontalOptions = Fit == FlexFit.Tight ? LayoutOptions.Fill : LayoutOptions.Center;
        Child.VerticalOptions = Fit == FlexFit.Tight ? LayoutOptions.Fill : LayoutOptions.Center;

        // Create a container to hold the child with proper padding and margin
        var container = new Grid
        {
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            Padding = Padding,
            Margin = Margin,
            Children = { Child }
        };

        Content = container;
    }

    #endregion
}

/// <summary>
/// Defines how a flexible widget fits into its parent.
/// </summary>
public enum FlexFit
{
    /// <summary>
    /// The child can be smaller than the available space.
    /// </summary>
    Loose,

    /// <summary>
    /// The child is forced to fill the available space.
    /// </summary>
    Tight
} 