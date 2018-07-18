using System;
using System.Text.RegularExpressions;

namespace Regexular
{
	[PureDI.Bean]
	public class Program
	{
		public static void Main()
		{
			var pdi = new PureDI.PDependencyInjector();
			Program prog = pdi.CreateAndInjectDependencies<Program>().rootBean;
			prog.DoOtherStuff();
		}
		private void DoOtherStuff()
		{
			Regex regex = new Regex(@"
				^
				(?<date>[^\[]*)
				\[
				(?<loggerlevel>[^\]]*)
				\]
				[ ]
				(?<category>[^:]+)
				:
				(?<eventcode>[^:]+)
				::
				(?<message>.+)
				::
				(?<url>.+)
				$
				"
				, RegexOptions.None | RegexOptions.IgnorePatternWhitespace);
			Match match = regex.Match("2018-07-18 14:38:29.335 +01:00 [Information] DasBlog.Managers.BlogManager: EntryAdded :: extra :: good :: http://localhost:50431/extra-good");
			Console.WriteLine($"there are {match.Groups.Count} groups");
			foreach (var group in match.Groups)
			{
				Console.WriteLine($"there are {((Group)group).Captures.Count} captures in this group");
				Console.WriteLine(((Group)group).Value);

			}
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
