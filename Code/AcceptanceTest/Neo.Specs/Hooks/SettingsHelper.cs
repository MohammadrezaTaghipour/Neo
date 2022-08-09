using Microsoft.Extensions.Configuration;

namespace Neo.Specs.Hooks
{
    public sealed class SettingsHelper
    {
        private static IConfigurationRoot _configuration;

        public static IConfigurationRoot GetConfiguration() => _configuration ??= CreateConfiguration();

        private static IConfigurationRoot CreateConfiguration()
        {
            var filePath = "appsettings.json";
            var env = Environment.GetEnvironmentVariable("ASPCORE_Environment");
            if (!string.IsNullOrEmpty(env)) filePath = $"appsettings.{env}.json";

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(filePath, false, true)
                .Build();
        }
    }
}