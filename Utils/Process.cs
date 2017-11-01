namespace com.mikedamay.Utils
{
	public class ProcessManager
	{
		public static void Main()
		{
			System.Console.WriteLine("hello process");
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.Arguments = "-i";
			psi.FileName = "/bin/bash";
			System.Diagnostics.Process.Start(psi);
			System.Console.WriteLine("Goodbye process");
		}
	}
}
