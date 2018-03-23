namespace Async
{
	using System.Threading.Tasks;
	using System.Threading;
	using System;

	public class Program 
	{
		public async static Task Main()
		{
			//Task<Mike> tm = DoSomething();
			//Mike mike = tm?.Result;
			//await DoSomething();
			Task t = DoThis();
			Console.WriteLine("In Main()");
			await t;
			Console.WriteLine("EXiting Main()");
		}
		private async static Task<Mike> DoSomething()
		{
			Mike mike = await Task.Run( () => {
				Thread.Sleep(10000);
				return new Mike();
			} );
			return mike;
		}
		private async static Task DoThis()
		{
			await Task.Yield();
			Thread.Sleep(1000);
			Console.WriteLine("In DoThis()");
		}
	}

	internal class Mike
	{
	}
}
		
