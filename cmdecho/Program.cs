using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace cmdecho
{
    // usage: one time do dotnet new console 
    //        copy in this file
    //        dotnet run
    class Program
    {
        private const string scriptName = "script.cmd";
        private static readonly string text = $"echo hear my echo"
          //+ $"{Environment.NewLine}set"  // uncomment line to see it work
          + $"{Environment.NewLine}timeout /t 1"
          + $"{Environment.NewLine}exit %errorlevel%"
          ;
        static void Main(string[] args)
        {
            File.WriteAllText(scriptName, text);
            var psi = new ProcessStartInfo("cmd.exe");
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.Arguments = "/C script.cmd";
            List<string> output = new List<string>();
            using (var ps = Process.Start(psi))
            {
                ps.OutputDataReceived += (sender, e) => output.Add(e.Data);
                ps.BeginOutputReadLine();
                ps.WaitForExit(5_000);
            }
            if (output.Count > 0)
            {
                Console.WriteLine("cmd.exe echo works well - what are you talking about");
            }
            else
            {
                Console.WriteLine("It's broken, I tell you");
            }
        }
    }
}
