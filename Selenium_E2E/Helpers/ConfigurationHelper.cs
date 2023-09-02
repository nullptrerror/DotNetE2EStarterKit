using Microsoft.Extensions.Configuration;
using NUnit_E2E.Settings;

namespace NUnit_E2E.Helpers
{
    public static class ConfigurationHelper
    {
        private static TestAppSettings testAppSettings = null!;
        public static TestAppSettings Build()
        {
            // if not null return the existing instance
            if (testAppSettings != null) return testAppSettings;

            // else create a new instance
            var config = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build();

            testAppSettings = new TestAppSettings();
            config.Bind(testAppSettings);

            return testAppSettings;
        }
    }
}