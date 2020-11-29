// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace McMaster.Extensions.CommandLineUtils
{
    /// <summary>
    /// The option used to determine if color output using ANSI commands should be enabled. This should only be used once per command line app.
    /// </summary>
    /// <remarks>
    /// Even when applied, console colors will remain disabled if:
    /// * the user has opted out of console colors by setting the NO_COLOR environment variable (https://no-color.org/)
    /// * the user is using Windows and the console cannot be set to support virtual terminal input.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ColorOptionAttribute : Attribute
    {
    }
}
