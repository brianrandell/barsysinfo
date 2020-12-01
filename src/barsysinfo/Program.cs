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
            bool IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            OutputWindowsSpecifics(IsWindows);
            OutputMacOsSpecifics(IsMacOS);
            OutputLinuxSpecifics(IsLinux);

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

        private static void OutputLinuxSpecifics(bool isLinux)
        {
            if (isLinux)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Getting Linux specific system information ...");
                Console.WriteLine(" ");

                var nestedArgs = "screenfetch";
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{nestedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                try
                {
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Console.WriteLine(output);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("screenfetch: command not found"))
                    {
                        Console.WriteLine("You need to install ScreenFetch for more details.");
                        return;
                    }
                }
            }
        }

        private static void OutputMacOsSpecifics(bool IsMacOS)
        {
            if (IsMacOS)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Getting macOS specific system information ...");

                var nestedArgs = "system_profiler SPSoftwareDataType";
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{nestedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine(output);
            }
        }

    }
}
