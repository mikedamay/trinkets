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
				while (true)
				{
					new EchoController().Echo(http);
					Console.Read();
				}
				http.Stop();
			}
		}
	}
}
namespace Listen.Traffic
{
	using System;
	using System.Net;
	using System.Threading;
	using System.IO;
	using System.Threading.Tasks;

	internal class EchoController
	{
		public async Task Echo(HttpListener http)
		{
			var context = await http.GetContextAsync();
			var request = context.Request;
			var response = context.Response;
			var s = response.OutputStream;
			using (StreamWriter sw = new StreamWriter(s))
			{
				Thread.Sleep(10000);
				sw.WriteLine(request.QueryString.Get(0));
			}
		}
	}
}
