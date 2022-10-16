using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NifKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] proccesses = Process.GetProcessesByName("BlackShot");
            int bsProcID = proccesses.FirstOrDefault().Id;
            if (bsProcID == 0)
            {
                Console.WriteLine("Failed To Find BlackShot Process");
                Console.ReadKey();
                Environment.Exit(0);
            }
            string handlepath = Directory.GetCurrentDirectory() + "\\handle.exe";
            Console.WriteLine(handlepath);
            // Query nif handle info
            if(!File.Exists(handlepath))
            {
                Console.WriteLine("Cannot find handle utility");
                Console.ReadKey();
                Environment.Exit(0);
            }
            string reply = Util.runCommand(handlepath, "-p BlackShot.exe");
            // Parse The Info
            List<string> _HandleIds = new List<string>();
            using (StringReader reader = new StringReader(reply))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    if (line.Contains(".nif"))
                    {
                        char[] Id = new char[4];
                        line.CopyTo(2, Id, 0, 4);
                        // Check for actual length because some handles either have 3 or 4 characters/digits
                        if (Id[Id.Length - 1] == ':')
                            _HandleIds.Add(new string(Id).Remove(3));
                        _HandleIds.Add(new string(Id));
                    }
                }
                if(_HandleIds.Count == 0)
                {
                    Console.WriteLine("Cannot find nif handles");
                    System.Threading.Thread.Sleep(4000);
                    Environment.Exit(0);

                }
                // Now that we have the nif handle ids we just call handle.exe with proper arguments and just close them
                foreach (string handleid in _HandleIds)
                {
                    Util.runCommand(handlepath, "-c 0x" + handleid + " -y -p " + bsProcID);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Killed All Nif Handles!");
                Console.ResetColor();
                System.Threading.Thread.Sleep(4000);
            }
        }
    }
}

