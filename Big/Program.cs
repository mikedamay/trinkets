namespace Big
{
	using System;
	using PureDI;

	[Bean]
	public class Program
	{
		public static void Main()
		{
			var pdi = new PDependencyInjector();
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			Console.WriteLine(@is.AllDiagnosticsToString());
		}
	}
}
namespace Big
{
	using System;
	using PureDI;
	using Microsoft.EntityFrameworkCore;

	internal class Repo
	{
		public void ReadDb()
		{
			var bdc = new BigDbContext();
		}
	}
	public class BigDbContext : DbContext
	{
	}
}
