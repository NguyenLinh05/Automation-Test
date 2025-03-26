using System;
using DotNetEnv;

namespace Enduser
{
    public static class EnvConfig
    {
        static EnvConfig()
        {
            // Load biến môi trường từ file .env
            Env.Load();
        }

        public static readonly string ChromeDriverPath = GetEnv("CHROME_DRIVER_PATH", "E:\\chromedriver-win64");

        private static string GetEnv(string key, string defaultValue)
        {
            return Environment.GetEnvironmentVariable(key) ?? defaultValue;
        }

    }
}
