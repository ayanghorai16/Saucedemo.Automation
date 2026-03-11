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
            if (!string.IsNullOrWhiteSpace(root)) return root;
            
            var baseDir = AppContext.BaseDirectory;
            // Try to find solution root by looking for .sln or .slnx
            var dir = new System.IO.DirectoryInfo(baseDir);
            while (dir != null && dir.GetFiles("*.sln*").Length == 0)
            {
                dir = dir.Parent;
            }
            var rootPath = dir?.FullName ?? baseDir;
            return System.IO.Path.Combine(rootPath, "TestArtifacts");
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
