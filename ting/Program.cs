using System;
using PureDI;
using PureDI.Attributes;
using System.IO;

namespace ting
{
	[Bean]
    public class Program
    {
		[BeanReference] private ILogger logger = null;
		[BeanReference] private Getter _getter = null;
		[BeanReference] private Bufferer _bufferer = null;
		[BeanReference] private TagsHandler _tagsHandler = null;
				// for the record - this is just playing
				// I'm not keen on this "action at a distance"
				// I have no reason to beleive that the tags
				// handler is in the correct condition when
				// I come to use it.
				// The fact is - I don't like DI.
        public static void Main(string[] args)
        {
			RunGetter();
        }
		private static void RunGetter()
		{
			DependencyInjector pdi = new DependencyInjector();
			(Program prog, InjectionState @is) = pdi.CreateAndInjectDependencies<Program>();
			Console.WriteLine(@is.Diagnostics.ToString());
			prog._getter.Get();
			//prog._bufferer.Buffer(new StreamReader(new FileStream("mike.xml"
			//  ,FileMode.Open, FileAccess.Read, FileShare.None)));
			prog.PrintResults();
		}
		private void PrintResults()
		{
			var tagSet = _tagsHandler.GetTags();
			foreach (var tag in tagSet.Keys)
			{
				Console.WriteLine($"{tag} {tagSet[tag]}");
			}
		}
    }
}
