using System;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main()
    {
		IHost host = new HostBuilder()
		.ConfigureLogging(
			logBuilder => {
				logBuilder.AddDebug();
				logBuilder.AddConsole();
				logBuilder.SetMinimumLevel(LogLevel.Debug);
			}
		)
		.ConfigureServices(
			(hostContext, services) => services.AddSingleton<Stooge>()
			
		)
		.Build();
        System.Console.WriteLine("Just stop the logger");
		var stooge = host.Services.GetService(typeof(Stooge));
		Console.WriteLine(stooge);
    }
}

public class Stooge
{
	public Stooge(ILogger<Stooge> logger)
	{
		Console.WriteLine("console works in Stooge");
		logger.LogInformation("nothing to see here");
		Thread.Sleep(5000);
	}
}
