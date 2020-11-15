// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace McMaster.Extensions.CommandLineUtils.HelpText
{
    internal struct ColorText
    {
        private const string EscapeCode = "\u001b[";
        public const string AnsiResetColorCommand = EscapeCode + "0m";
        private readonly string _text;

        public ColorText(string text, ConsoleColor? color, ConsoleColor? backgroundColor = null)
        {
            _text = AnsiColorCommand(color, backgroundColor) + text + AnsiResetColorCommand;
        }

        public override string ToString() => _text;

        public static implicit operator string(ColorText colorText)
        {
            return colorText._text;
        }

        public static string AnsiColorCommand(ConsoleColor? color, ConsoleColor? backgroundColor = null)
        {
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
