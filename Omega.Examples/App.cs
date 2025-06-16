using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Omega.Controls.Basic;
using Omega;

namespace Omega.Examples;

public class App : Application
{
	public App()
	{
		var services = new ServiceCollection();
		
		// Register all injectable services (including handlers)
		services.AddInjectableServices();
		
		// Register the MainPage
		services.AddTransient<MainPage>();
		
		// Build the service provider
		var serviceProvider = services.BuildServiceProvider();
		
		// Set the main page using dependency injection
		MainPage = serviceProvider.GetRequiredService<MainPage>();
	}
} 