namespace sock
{
	using System;
	using PureDI;

	[Bean]
	public class Program
	{
		[BeanReference] private IConverser _messageHandler = null;
		public static void Main(string[] args)
		{
			string[] profiles = args.Length > 0 ? new[] {args[0]} : null;
			var pdi = new PDependencyInjector(Profiles: profiles );
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			Console.WriteLine(@is.AllDiagnosticsToString());
			prog._messageHandler.Converse();
		}
		private void DoStuff()
		{
		}
	}
	public interface IConverser
	{
		void Converse();
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
	internal class Client : IConverser
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
namespace sock
{
	using System;
	using System.Net.Sockets;
	using PureDI;
	using static System.Text.Encoding;
	using System.Net;
	using System.IO;
	using System.Linq;

	[Bean(Profile="server")]
	internal class Server : TcpListener,  IConverser
	{
		public Server() : base(new IPEndPoint(
		  new IPAddress(new byte[] {127, 0, 0, 1}), 11900))
		{
		}
		public void Converse()
		{
			try
			{
				Start();
				TcpClient client = AcceptTcpClient();
				NetworkStream s = client.GetStream();
				s.Write(ASCII.GetBytes("hello from TcpListener"), 0, "hello from TcpListener".Length);
				byte[] by = new byte[4096];
				int bytesRead = s.Read(by, 0, by.Length);
				int stringLength = by.Select((b,i) 
				  => new {@byte = b, index = i}).First(bi => bi.@byte == 0).index;
				Console.WriteLine(ASCII.GetString(by, 0, bytesRead));
			}
			catch (Exception ex)
			{
				throw new Exception("bad stuff: ", ex);	
			}
			finally
			{
				Stop();
			}
		}
	}
}
