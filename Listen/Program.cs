namespace Listen
{
	using System;
	using PureDI;
	using Listen.Traffic;

	[Bean]
	public class Program
	{
		[BeanReference] private Trafficker _trafficker = null;
		public static void Main()
		{
			var pdi = new PDependencyInjector();
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			Console.WriteLine(@is.WarningsToString());
			prog._trafficker.Flow();	
		}
	}
	internal interface ITrafficker
	{
		void Flow();
	}
}
namespace Listen.Traffic
{
	using System;
	using PureDI;
	using System.Net;
	using System.Text;
	using System.IO;
	using System.Threading;

	[Bean]
	internal class Trafficker : ITrafficker
	{
		public void Flow()
		{
			Console.WriteLine("starting listener");
			using( var http = new HttpListener())
			{
				http.Prefixes.Add("http://*:8080/");
				http.Start();
				var context = http.GetContext();
				var request = context.Request;
				var response = context.Response;
				var s = response.OutputStream;
				using (StreamWriter sw = new StreamWriter(s))
				{
					sw.WriteLine("Mike is great<br>");
					sw.WriteLine("other stuff");
				}
				Thread.Sleep(1000);
				http.Stop();
			}
		}
	}
}
