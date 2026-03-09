using System;
using Microsoft.Playwright;

namespace Saucedemo.Framework.Core;

public static class Config
{
    public static string BaseUrl => "https://www.saucedemo.com/";
    public static string Browser => (Environment.GetEnvironmentVariable("BROWSER") ?? "chrome").ToLowerInvariant();
    public static bool Headless
    {
        get
        {
            var v = Environment.GetEnvironmentVariable("HEADLESS");
            if (string.IsNullOrWhiteSpace(v)) return false;
            v = v.ToLowerInvariant();
            return v is "1" or "true" or "yes";
        }
    }
    public static string ArtifactsRoot
    {
        get
        {
            var root = Environment.GetEnvironmentVariable("ARTIFACTS_DIR");
            return string.IsNullOrWhiteSpace(root) ? System.IO.Path.Combine(AppContext.BaseDirectory, "artifacts") : root;
        }
    }
    public static BrowserTypeLaunchOptions LaunchOptions()
    {
        var headless = Headless;
        var opts = new BrowserTypeLaunchOptions
        {
            Headless = headless,
            Timeout = 60000
        };
        return opts;
    }
}
