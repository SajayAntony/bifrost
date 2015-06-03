using System;
using System.Diagnostics;
using System.IO;
using Web.App_Start;

namespace Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:8080";
            OwinSelfhostStartup.Startup(baseAddress);
            Console.WriteLine("Sample Usage:");            
            //curl --request PUT 'http://localhost:8080/resource' -H "Content-Type:application/json" -H "Accept: application/json" --data "{name:'Bridge.Commands.Hostname'}"
            Console.WriteLine("curl --request PUT 'http://localhost:8080/resource' -H \"Content-Type:application/json\" -H \"Accept: application/json\" --data \"{name:'Bridge.Commands.Hostname'}\"");
            Test();
            Console.ReadLine();
        }

        [Conditional("DEBUG")]
        static void Test()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("powershell.exe", "-ExecutionPolicy Bypass -File " + Path.GetFullPath("ensureBridge.ps1"));
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            var proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            proc.WaitForExit();
            string result = proc.StandardOutput.ReadToEnd();
            Console.WriteLine("Result from Test: " + result);
        }
    }
}
