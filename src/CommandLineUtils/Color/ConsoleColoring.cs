// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace McMaster.Extensions.CommandLineUtils.Color
{
    internal static class ConsoleColoring
    {
        private const int STD_OUTPUT_HANDLE = -11;

        // https://docs.microsoft.com/en-us/windows/console/setconsolemode#parameters
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        
        // https://docs.microsoft.com/en-us/windows/console/getconsolemode
        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        // https://docs.microsoft.com/en-us/windows/console/setconsolemode
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        public static bool IsEnabled { get; private set; }

        private static bool AnsiCommandsSupported { get; } = DetermineAnsiCommandSupport();

        public static bool Enable()
        {
            IsEnabled = AnsiCommandsSupported;
            return IsEnabled;
        }

        public static void Disable()
        {
            IsEnabled = false;
        }

        private static bool DetermineAnsiCommandSupport()
        {
            if (Environment.GetEnvironmentVariable("NO_COLOR") != null)
            {
                // Support the no-color manifesto https://no-color.org/
                return false;
            }

            var enabled = false;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var stdOut = GetStdHandle(STD_OUTPUT_HANDLE);

                if (GetConsoleMode(stdOut, out var outputConsoleMode))
                {
                    enabled = SetConsoleMode(stdOut, outputConsoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
                }
            }
            else
            {
                // For now assume that on other OS' the terminal supports ANSI commands
                enabled = true;
            }

            return enabled;
        }
    }
}
