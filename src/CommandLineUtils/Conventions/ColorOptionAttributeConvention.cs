// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;

using McMaster.Extensions.CommandLineUtils.Color;

namespace McMaster.Extensions.CommandLineUtils.Conventions
{
    /// <summary>
    /// Enables console coloring using <see cref="ColorText"/> based on the usage of <see cref="ColorOptionAttribute"/>.
    /// </summary>
    public class ColorOptionAttributeConvention : OptionAttributeConventionBase<HelpOptionAttribute>, IConvention
    {
        /// <inheritdoc />
        public void Apply(ConventionContext context)
        {
            if (context.ModelType == null)
            {
                return;
            }

            if (context.ModelType.GetCustomAttribute<ColorOptionAttribute>() != null)
            {
                ConsoleColoring.Enable();
            }
        }
    }
}
