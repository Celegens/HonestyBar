using System;
using Microsoft.Extensions.Configuration;

namespace HonestyBar.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddIf(this IConfigurationBuilder configurationBuilder, bool condition,
            Func<IConfigurationBuilder, IConfigurationBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return condition ? action(configurationBuilder) : configurationBuilder;
        }
    }
}