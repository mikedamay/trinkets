namespace sock
{
	using System;
	using PureDI;

	[Bean]
	public class Program
	{
		[BeanReference] private Client _client = null;
		public static void Main()
		{
			var pdi = new PDependencyInjector();
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			Console.WriteLine(@is.AllDiagnosticsToString());
			prog._client.Converse();
		}
		private void DoStuff()
		{
		}
	}
}
namespace sock
{
	using System;
	using PureDI;
	using System.Net.Sockets;
	using System.Net;
	using System.Text;
	using System.Linq;

	[Bean]
	internal class Client
	{
		public void Converse()
		{
			using (var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				s.Connect(new IPEndPoint(new IPAddress(new byte[] {127, 0, 0, 1}), 11900));
				s.Send(Encoding.UTF8.GetBytes("a cs string"));
				byte[] by = new byte[4096];
				int bytesRead = s.Receive(by);
				int stringLength = by.Select((b, i) => new {@byte = b, index = i}).First((bi) => bi.@byte == 0).index;
				Console.WriteLine($"bytesRead=={bytesRead}");
				Console.WriteLine($"stringLength=={stringLength}");
				Console.WriteLine(Encoding.UTF8.GetString(by, 0, stringLength));
			}
		}
	}
}
