namespace com.mikedamay.Utils
{
	public class ProcessManager
	{
		public static void Main()
		{
			System.Console.WriteLine("hello process");
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.CreateNoWindow = true;
			psi.Arguments = "--verbose -i";
			psi.FileName = "/bin/bash";
			psi.RedirectStandardInput = true;
			var p = new System.Diagnostics.Process();
			p.StartInfo = psi;
			p.Start();
			p.StandardInput.WriteLine("echo $SHLVL;sleep 5");
			p.StandardInput.Close();
			p.WaitForExit();
			System.Console.WriteLine("Goodbye process");
		}
	}
}
