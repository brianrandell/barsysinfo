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

            Console.WriteLine(" ");
            Console.WriteLine("Press any key to exit program.");
            Console.ReadLine();
        }
    }
}
