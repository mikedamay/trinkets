using System;
using System.IO;

namespace ting
{
	public class Reader
	{
		public void Read()
		{
			using (var sr = new StreamReader(new FileStream("mike.txt", FileMode.Open, FileAccess.Read, FileShare.None)))
			{
				string s;
				while ((s = sr.ReadLine()) != null)
				{
					string @out  = s.Convert(st => st);
					Console.WriteLine(@out);
				}
			}
		}
	}
	public static class Extensions
	{
		public static T Convert<T>(this string str, Func<string, T> converter)
		{
			return converter(str);
		}
	}
}
