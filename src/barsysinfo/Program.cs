using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.Versioning;
using System.Diagnostics;
using System.IO;

namespace barsysinfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Getting system information ...");
            Console.WriteLine(" ");

            var frameworkName = Assembly
                .GetEntryAssembly()?
                .GetCustomAttribute<TargetFrameworkAttribute>()?
                .FrameworkName;

            Console.WriteLine($"MachineName: {Environment.MachineName}");
            Console.WriteLine($"OSVersion: {Environment.OSVersion}");
            Console.WriteLine($"Version: {Environment.Version}");
            Console.WriteLine($"FrameworkName: {frameworkName}");
            Console.WriteLine($"Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");

            bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            OutputWindowsSpecifics(IsWindows);

            Console.WriteLine(" ");
            Console.WriteLine("Press any key to exit program.");
            Console.ReadLine();
        }

        private static void OutputWindowsSpecifics(bool isWindows)
        {
            if (isWindows)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Getting Windows specific system information ...");
                Console.WriteLine(" ");

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "SystemInfo",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                string currentLine;
                using (StringReader sr = new StringReader(output))
                {
                    while (sr.Peek() > -1)
                    {
                        currentLine = sr.ReadLine();
                        if (!currentLine.StartsWith("Hotfix(s):"))
                        {
                            Console.WriteLine(currentLine);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
