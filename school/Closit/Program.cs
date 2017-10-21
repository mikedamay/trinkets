namespace Closit
{
	[PureDI.Bean]
	public class Program
	{
		[PureDI.BeanReference] private Closit _closit = null;
		public static void Main()
		{
			var pdi = new PureDI.PDependencyInjector();
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			System.Console.WriteLine(@is.AllDiagnosticsToString());
			prog._closit.DoStuff();
		}
	}
}
namespace Closit
{
	using PureDI;
	using System;
	using TopTimer = System.Timers.Timer;
	using System.Threading;
	[Bean]
	internal class Closit
	{
		private TopTimer timer = new TopTimer();
		public void DoStuff()
		{
			for (int ii = 0; ii < 10; ii++)
			{
				timer.Interval = 1000;
				timer.AutoReset = false;
				timer.Enabled = true;
				timer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) 
				  => Console.WriteLine($"hello {ii}");			
			}
			Console.WriteLine("hello Closit");
			while (true)
			{
				Thread.Sleep(500);
			}
		}
	}
}
