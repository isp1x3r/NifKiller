using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NifKiller
{
    internal static class Util
    {
        internal static string runCommand(string filename, string argument)
        {
            Process process = new Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = argument; // Note the /c command (*)
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

    }
}
