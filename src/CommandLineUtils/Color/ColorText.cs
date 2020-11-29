// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace McMaster.Extensions.CommandLineUtils.Color
{
    /// <summary>
    /// Provides support for emitting colored text to the console using virtual terminal (VT) commands. If text coloring
    /// is not enabled no commands will be added to the text.
    /// </summary>
    public struct ColorText
    {
        private const string EscapeCode = "\u001b[";
        private const string Reset = EscapeCode + "0m";
        private readonly string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorText"/> struct.
        /// </summary>
        /// <param name="text">
        /// The text to add color commands to.
        /// </param>
        /// <param name="color">
        ///
        /// </param>
        /// <param name="backgroundColor"></param>
        public ColorText(string text, ConsoleColor? color, ConsoleColor? backgroundColor = null)
        {
            _text = AnsiColorCommand(color, backgroundColor) + text + AnsiResetColorCommand();
        }

        /// <inheritdoc />
        public override string ToString() => _text;

        /// <summary>
        /// Converts a <see cref="ColorText"/> instance to a string.
        /// </summary>
        /// <param name="colorText">
        /// The instance to convert.
        /// </param>
        public static implicit operator string(ColorText colorText)
        {
            return colorText._text;
        }

        /// <summary>
        /// Generates the virtual terminal command to reset the console colors back to their defaults. This can
        /// be used as an alternative to a <see cref="ColorText"/> instance when commands need to be appended to text builders,
        /// e.g. a <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <returns>
        /// The virtual terminal command.
        /// </returns>
        public static string AnsiResetColorCommand()
        {
            return ConsoleColoring.IsEnabled ? Reset : string.Empty;
        }

        /// <summary>
        /// Generates the virtual terminal commands required to switch the console to the given color configuration. This can
        /// be used as an alternative to a <see cref="ColorText"/> instance when commands need to be appended to text
        /// builders, e.g. a <see cref="System.Text.StringBuilder"/>.
        /// </summary>
        /// <param name="color">
        /// The foreground color to use.
        /// </param>
        /// <param name="backgroundColor">
        /// The optional background color to use.
        /// </param>
        /// <returns>
        /// The virtual terminal commands.
        /// </returns>
        public static string AnsiColorCommand(ConsoleColor? color, ConsoleColor? backgroundColor = null)
        {
            if (!ConsoleColoring.IsEnabled)
            {
                return string.Empty;
            }

            static string FormatColorCommand((int ansiColorCode, string modifier) command) => $"{EscapeCode}{command.ansiColorCode}{command.modifier}m";

            string? foregroundColorCommand = color == null
                ? string.Empty
                : FormatColorCommand(GetForegroundAnsiCode(color.GetValueOrDefault()));

            string? backgroundColorCommand = backgroundColor == null
                ? string.Empty
                : FormatColorCommand(GetBackgroundAnsiCode(backgroundColor.GetValueOrDefault()));

            return $"{foregroundColorCommand}{backgroundColorCommand}";
        }

        private static (int ansiCode, string modifierText) GetForegroundAnsiCode(ConsoleColor color)
        {
            bool isBrightColor = color >= ConsoleColor.DarkGray; // Dark gray is actually "bright black"
            string? brightColorModifier = isBrightColor ? ";1" : string.Empty;
            int ansiCode = color switch
            {
                ConsoleColor.Black => 30,
                ConsoleColor.DarkBlue => 34,
                ConsoleColor.DarkGreen => 32,
                ConsoleColor.DarkCyan => 36,
                ConsoleColor.DarkRed => 31,
                ConsoleColor.DarkMagenta => 35,
                ConsoleColor.DarkYellow => 33,
                ConsoleColor.Gray => 37,
                ConsoleColor.DarkGray => 30,
                ConsoleColor.Blue => 34,
                ConsoleColor.Green => 32,
                ConsoleColor.Cyan => 36,
                ConsoleColor.Red => 31,
                ConsoleColor.Magenta => 35,
                ConsoleColor.Yellow => 33,
                ConsoleColor.White => 30,
                _ => throw new ArgumentException(nameof(color)),
            };

            return (ansiCode, brightColorModifier);
        }

        private static (int ansiCode, string modifierText) GetBackgroundAnsiCode(ConsoleColor color)
        {
            // Background codes are foreground colors, offset by 10
            (int ansiCode, string modifierText) = GetForegroundAnsiCode(color);
            return (ansiCode + 10, modifierText);
        }
    }
}
