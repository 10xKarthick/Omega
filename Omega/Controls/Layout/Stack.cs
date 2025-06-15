using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Omega.Controls.Layout;

/// <summary>
/// A widget that positions its children relative to the edges of its box.
/// </summary>
public class Stack : ContentView
{
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the collection of child views in the stack.
    /// </summary>
    public static readonly BindableProperty ChildrenProperty =
        BindableProperty.Create(
            nameof(Children),
            typeof(IList<View>),
            typeof(Stack),
            new List<View>(),
            propertyChanged: (bindable, oldVal, newVal) => ((Stack)bindable).UpdateStack());

    /// <summary>
    /// Gets or sets the horizontal options for the stack.
    /// </summary>
    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Stack),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Stack)bindable).UpdateStack());

    /// <summary>
    /// Gets or sets the vertical options for the stack.
    /// </summary>
    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Stack),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Stack)bindable).UpdateStack());

    /// <summary>
    /// Gets or sets the padding of the stack.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Stack),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Stack)bindable).UpdateStack());

    /// <summary>
    /// Gets or sets the margin of the stack.
    /// </summary>
    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Stack),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Stack)bindable).UpdateStack());

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Stack"/> class.
    /// </summary>
    public Stack()
    {
        UpdateStack();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the collection of child views in the stack.
    /// </summary>
    public IList<View> Children
    {
        get => (IList<View>)GetValue(ChildrenProperty);
        set => SetValue(ChildrenProperty, value);
    }

    /// <summary>
    /// Gets or sets the horizontal options for the stack.
    /// </summary>
    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical options for the stack.
    /// </summary>
    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the stack.
    /// </summary>
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the stack.
    /// </summary>
    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    #endregion

    #region Private Methods

    private void UpdateStack()
    {
        var grid = new Grid
        {
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            Padding = Padding,
            Margin = Margin
        };

        if (Children != null)
        {
            foreach (var child in Children.Where(c => c != null))
            {
                grid.Children.Add(child);
            }
        }

        Content = grid;
    }

    #endregion
} 