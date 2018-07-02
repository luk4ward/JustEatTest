using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace JustEatTest.Api
{
    public static class ConfigurationExtensions
    {
        public static string Dump(this IConfiguration configuration)
        {
            var log = new StringBuilder();

            log.Append("Configuration");
            log.AppendLine();

            foreach (var section in configuration.GetChildren())
            {
                DumpSection(section, log, 0, true);
            }

            return log.ToString();
        }

        private static void DumpSection(IConfigurationSection section, StringBuilder log, int depth, bool rootSection = false)
        {
            log.Append('\t');
            log.Append(' ', depth * 2);
            log.AppendFormat("{0}: {1}\n", section.Key, section.Value);

            foreach (var child in section.GetChildren())
            {
                DumpSection(child, log, depth + 1);
            }

            if (rootSection)
                log.AppendLine();
        }
    }
}