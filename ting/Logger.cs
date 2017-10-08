using System;
using PureDI;

namespace ting
{
	internal interface ILogger
	{
		void Log(string str, Exception exception = null);
	}
	[Bean]
	internal class Logger : ILogger
	{
		public void Log(string str, Exception exception = null)
		{
			Console.WriteLine(str);
			if (exception != null)
			{
				Console.WriteLine(exception.StackTrace);
			}
		}
	}
}
