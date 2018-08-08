namespace Async
{
	using System.Threading.Tasks;
	using System.Threading;
	using System;

	public class Program 
	{
		public async static Task Main()
		{
			const int NumTasks = 10;
			Task[] tasks = new Task[NumTasks];
			for (int ii = 0; ii < NumTasks; ii++ )
			{
				tasks[ii] = DoThis(ii);
				//await DoThis(ii);
			}	
			Task t = Task.WhenAll(tasks);
			Console.WriteLine("In Main()");
			Console.WriteLine("EXiting Main()");
			await t;
		}
		private async static Task DoThis(int n)
		{
			await Task.Yield();
			Thread.Sleep(1000);
			Console.WriteLine($"In DoThis({Thread.GetCurrentProcessorId()})");
			//Console.WriteLine($"In DoThis({n})");
		}
	}
}
		
