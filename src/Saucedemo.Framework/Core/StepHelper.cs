using System;
using System.IO;
using System.Threading.Tasks;
using Allure.Commons;
using Microsoft.Playwright;

namespace Saucedemo.Framework.Core;

public static class StepHelper
{
    public static async Task<string> Capture(IPage page, string testName, string step)
    {
        var root = Config.ArtifactsRoot;
        var dir = Path.Combine(root, "AllureReportScreenshots", Sanitize(testName));
        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, $"{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}_{Sanitize(step)}.png");
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = file, FullPage = true });
        return file;
    }
    public static void AttachText(string name, string content)
    {
        try
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(content);
            AllureLifecycle.Instance.AddAttachment(name, "text/plain", bytes, "txt");
        }
        catch
        {
        }
    }
    static string Sanitize(string s)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            s = s.Replace(c, '_');
        }
        return s.Replace(" ", "_");
    }
}
