using System;
using System.IO;
using System.Collections.Generic;

namespace ting
{
	public class Reader
	{
		public IEnumerable<TResult> Read<TResult>(Func<string, TResult> convert)
		{
			using (var sr = new StreamReader(new FileStream("mike.txt", FileMode.Open, FileAccess.Read, FileShare.None)))
			{
				string s;
				while ((s = sr.ReadLine()) != null)
				{
					TResult @out  = s.Convert(convert);
					yield return @out;
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
