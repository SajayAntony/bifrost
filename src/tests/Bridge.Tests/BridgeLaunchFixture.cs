﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bridge.Tests
{
    class BridgeLaunchFixture
    {
        public BridgeLaunchFixture()
        {

            ProcessStartInfo procStartInfo = new ProcessStartInfo("powershell.exe",
                        "-ExecutionPolicy Bypass -File " + 
                        Path.GetFullPath("artifacts\\ensureBridge.ps1"));
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            var proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            proc.WaitForExit();
            string result = proc.StandardOutput.ReadToEnd();
            Console.WriteLine("Result from Test: " + result);

            if (proc.ExitCode != 0) {
                throw new InvalidOperationException("Could not launch Bridge process.");
            }
        }
    }
}
