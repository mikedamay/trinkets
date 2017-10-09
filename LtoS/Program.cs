using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LtoS
{
	public class Program
	{
		public static void Main()
		{
			Program prog = new Program();
			prog.DoStuff<int>(t => t != 0, 1, 2, 3, 4);
		}
		private void DoStuff<T>( Func<T, bool> compare,  params T[] init)
		{
			Console.WriteLine("abc");
			var list = init.ToList();
			var s = list.ToStream();
			T line;
			while(compare(line = s.Read()))
			{
				Console.WriteLine(line);
			}
		}
	}
	internal static class Extensions
	{
		public static EnumStream<TSource> ToStream<TSource>(this IEnumerable<TSource> source)
		{
			return new EnumStream<TSource>(source);
		}
	}
}	
