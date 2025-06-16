using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Omega.Controls.Basic;
using Omega.Controls.Layout;

namespace Omega.Examples;

public class MainPage : ContentPage
{
	public MainPage(Container container)
	{
		Title = "Omega Controls Examples";

		var scrollView = new ScrollView
		{
			Content = new Stack
			{
				Child = new Stack
				{
					Child = new Stack
					{
						Child = CreateExamples(),
						Padding = new Thickness(16),
						Spacing = 16
					},
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					MaxWidth = 600
				},
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill
			}
		};

		Content = scrollView;
	}

	private View CreateExamples()
	{
		return new Stack
		{
			Child = new Stack
			{
				Children =
				{
					CreateHeader("Basic Container Examples"),
					CreateBasicContainerExamples(),
					CreateHeader("Container with Border Examples"),
					CreateBorderContainerExamples(),
					CreateHeader("Container with Corner Radius Examples"),
					CreateCornerRadiusContainerExamples(),
					CreateHeader("Container with Background Examples"),
					CreateBackgroundContainerExamples(),
					CreateHeader("Container with Padding Examples"),
					CreatePaddingContainerExamples()
				},
				Spacing = 24
			}
		};
	}

	private View CreateHeader(string text)
	{
		return new Label
		{
			Text = text,
			FontSize = 24,
			FontAttributes = FontAttributes.Bold,
			TextColor = Colors.Black,
			Margin = new Thickness(0, 16, 0, 8)
		};
	}

	private View CreateBasicContainerExamples()
	{
		return new Stack
		{
			Children =
			{
				new Container
				{
					Child = new Label
					{
						Text = "Basic Container",
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center,
						Padding = new Thickness(16)
					},
					BackgroundColor = Colors.White,
					BorderColor = Colors.Gray,
					BorderWidth = 1,
					CornerRadius = new CornerRadius(8),
					Padding = new Thickness(16)
				},
				new Container
				{
					Child = new Label
					{
						Text = "Container with Shadow",
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center,
						Padding = new Thickness(16)
					},
					BackgroundColor = Colors.White,
					BorderColor = Colors.Gray,
					BorderWidth = 1,
					CornerRadius = new CornerRadius(8),
					Padding = new Thickness(16),
					Margin = new Thickness(0, 16, 0, 0)
				}
			},
			Spacing = 16
		};
	}

	private View CreateBorderContainerExamples()
	{
		return new Stack
		{
			Children =
			{
				CreateBorderContainer(Colors.Blue, 1),
				CreateBorderContainer(Colors.Red, 2),
				CreateBorderContainer(Colors.Green, 3),
				CreateBorderContainer(Colors.Purple, 4)
			},
			Spacing = 16
		};
	}

	private View CreateBorderContainer(Color color, double width)
	{
		return new Container
		{
			Child = new Label
			{
				Text = $"Border Width: {width}",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(16)
			},
			BackgroundColor = Colors.White,
			BorderColor = color,
			BorderWidth = width,
			CornerRadius = new CornerRadius(8),
			Padding = new Thickness(16)
		};
	}

	private View CreateCornerRadiusContainerExamples()
	{
		return new Stack
		{
			Children =
			{
				CreateCornerRadiusContainer(0),
				CreateCornerRadiusContainer(8),
				CreateCornerRadiusContainer(16),
				CreateCornerRadiusContainer(24)
			},
			Spacing = 16
		};
	}

	private View CreateCornerRadiusContainer(double radius)
	{
		return new Container
		{
			Child = new Label
			{
				Text = $"Corner Radius: {radius}",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(16)
			},
			BackgroundColor = Colors.White,
			BorderColor = Colors.Gray,
			BorderWidth = 1,
			CornerRadius = new CornerRadius(radius),
			Padding = new Thickness(16)
		};
	}

	private View CreateBackgroundContainerExamples()
	{
		return new Stack
		{
			Children =
			{
				CreateBackgroundContainer(Colors.LightBlue),
				CreateBackgroundContainer(Colors.LightGreen),
				CreateBackgroundContainer(Colors.LightPink),
				CreateBackgroundContainer(Colors.LightYellow)
			},
			Spacing = 16
		};
	}

	private View CreateBackgroundContainer(Color color)
	{
		return new Container
		{
			Child = new Label
			{
				Text = $"Background: {color}",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(16)
			},
			BackgroundColor = color,
			BorderColor = Colors.Gray,
			BorderWidth = 1,
			CornerRadius = new CornerRadius(8),
			Padding = new Thickness(16)
		};
	}

	private View CreatePaddingContainerExamples()
	{
		return new Stack
		{
			Children =
			{
				CreatePaddingContainer(8),
				CreatePaddingContainer(16),
				CreatePaddingContainer(24),
				CreatePaddingContainer(32)
			},
			Spacing = 16
		};
	}

	private View CreatePaddingContainer(double padding)
	{
		return new Container
		{
			Child = new Label
			{
				Text = $"Padding: {padding}",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(16)
			},
			BackgroundColor = Colors.White,
			BorderColor = Colors.Gray,
			BorderWidth = 1,
			CornerRadius = new CornerRadius(8),
			Padding = new Thickness(padding)
		};
	}
} 