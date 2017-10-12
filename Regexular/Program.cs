using System;
using System.Text.RegularExpressions;

namespace Regexular
{
	[PureDI.Bean]
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine(ii);
			var pdi = new PureDI.PDependencyInjector();
			Program prog = pdi.CreateAndInjectDependencies<Program>().rootBean;
			prog.DoStuff();
		}
		private void DoStuff()
		{
			Regex regex = new Regex("(.*)string(.*)", RegexOptions.None);
			Match match = regex.Match("some string of my making");
			Console.WriteLine($"there are {match.Groups.Count} groups");
			foreach (var group in match.Groups)
			{
				Console.WriteLine($"there are {((Group)group).Captures.Count} captures in this group");
				Console.WriteLine(((Group)group).Value);

			}
		}
	}
}
