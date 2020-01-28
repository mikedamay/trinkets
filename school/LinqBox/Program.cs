using System;
using System.Linq;

public class Program
{
	public static void Main()
	{
		int ii = 0;
		int[] aa = { 1, 2, 3, 4 };
		var list = aa.Select( a => { (ii, _) = PassForward(ii, a); return (a, ii);} ).ToList();
		Console.WriteLine( $"ii == {ii}" );
		foreach (var ll in list)
		{
			Console.WriteLine( $"item == {ll}");
		}
	}
	private static (int, string) PassForward(int jj, int kk)
	{
		return (jj + kk, jj.ToString());
	}
}

