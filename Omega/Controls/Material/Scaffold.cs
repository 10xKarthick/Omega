using Microsoft.Maui.Controls;
using Omega.Controls.Basic;

namespace Omega.Controls.Material;

public class Scaffold : ContentView
{
    public static readonly BindableProperty AppBarProperty =
        BindableProperty.Create(
            nameof(AppBar),
            typeof(View),
            typeof(Scaffold),
            default(View),
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty BodyProperty =
        BindableProperty.Create(
            nameof(Body),
            typeof(View),
            typeof(Scaffold),
            default(View),
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty FloatingActionButtonProperty =
        BindableProperty.Create(
            nameof(FloatingActionButton),
            typeof(View),
            typeof(Scaffold),
            default(View),
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty BottomNavigationBarProperty =
        BindableProperty.Create(
            nameof(BottomNavigationBar),
            typeof(View),
            typeof(Scaffold),
            default(View),
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(
            nameof(BackgroundColor),
            typeof(Color),
            typeof(Scaffold),
            Colors.White,
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Scaffold),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty MarginProperty =
        BindableProperty.Create(
            nameof(Margin),
            typeof(Thickness),
            typeof(Scaffold),
            new Thickness(0),
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(Scaffold),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    public static readonly BindableProperty VerticalOptionsProperty =
        BindableProperty.Create(
            nameof(VerticalOptions),
            typeof(LayoutOptions),
            typeof(Scaffold),
            LayoutOptions.Fill,
            propertyChanged: (bindable, oldVal, newVal) => ((Scaffold)bindable).UpdateScaffold());

    private readonly Grid _grid;
    private readonly Grid _fabContainer;

    public Scaffold()
    {
        _grid = new Grid
        {
            HorizontalOptions = HorizontalOptions,
            VerticalOptions = VerticalOptions,
            BackgroundColor = BackgroundColor,
            Padding = Padding,
            Margin = Margin
        };

        // Add row definitions
        _grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // AppBar row
        _grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // Body row
        _grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // BottomNavigationBar row

        // Add column definitions for FAB positioning
        _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // Main content
        _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // FAB column

        // Create FAB container
        _fabContainer = new Grid
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.End,
            Padding = new Thickness(16),
            Margin = new Thickness(16)
        };

        Content = _grid;
        UpdateScaffold();
    }

    public View AppBar
    {
        get => (View)GetValue(AppBarProperty);
        set => SetValue(AppBarProperty, value);
    }

    public View Body
    {
        get => (View)GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    public View FloatingActionButton
    {
        get => (View)GetValue(FloatingActionButtonProperty);
        set => SetValue(FloatingActionButtonProperty, value);
    }

    public View BottomNavigationBar
    {
        get => (View)GetValue(BottomNavigationBarProperty);
        set => SetValue(BottomNavigationBarProperty, value);
    }

    public Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    public LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    public LayoutOptions VerticalOptions
    {
        get => (LayoutOptions)GetValue(VerticalOptionsProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    private void UpdateScaffold()
    {
        if (_grid != null)
        {
            _grid.Children.Clear();
            _fabContainer.Children.Clear();

            // Add AppBar
            if (AppBar != null)
            {
                Grid.SetRow(AppBar, 0);
                Grid.SetColumn(AppBar, 0);
                Grid.SetColumnSpan(AppBar, 2);
                _grid.Children.Add(AppBar);
            }

            // Add Body
            if (Body != null)
            {
                Grid.SetRow(Body, 1);
                Grid.SetColumn(Body, 0);
                Grid.SetColumnSpan(Body, 2);
                _grid.Children.Add(Body);
            }

            // Add FloatingActionButton
            if (FloatingActionButton != null)
            {
                _fabContainer.Children.Add(FloatingActionButton);
                Grid.SetRow(_fabContainer, 1);
                Grid.SetColumn(_fabContainer, 1);
                _grid.Children.Add(_fabContainer);
            }

            // Add BottomNavigationBar
            if (BottomNavigationBar != null)
            {
                Grid.SetRow(BottomNavigationBar, 2);
                Grid.SetColumn(BottomNavigationBar, 0);
                Grid.SetColumnSpan(BottomNavigationBar, 2);
                _grid.Children.Add(BottomNavigationBar);
            }

            _grid.BackgroundColor = BackgroundColor;
            _grid.Padding = Padding;
            _grid.Margin = Margin;
            _grid.HorizontalOptions = HorizontalOptions;
            _grid.VerticalOptions = VerticalOptions;
        }
    }
} 