using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Saucedemo.Framework.Core;

public sealed class PlaywrightDriver : IAsyncDisposable
{
    public IPlaywright? Playwright { get; private set; }
    public IBrowser? Browser { get; private set; }
    public IBrowserContext? Context { get; private set; }
    public IPage? Page { get; private set; }

    public async Task InitializeAsync()
    {
        var artifactsRoot = Config.ArtifactsRoot;
        Directory.CreateDirectory(artifactsRoot);
        var runId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var traceDir = Path.Combine(artifactsRoot, "traces", runId);
        Directory.CreateDirectory(traceDir);

        Microsoft.Playwright.Program.Main(new[] { "install" });
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        var opts = Config.LaunchOptions();
        var browser = Config.Browser;
        switch (browser)
        {
            case "chrome":
                opts.Channel = "chrome";
                Browser = await Playwright.Chromium.LaunchAsync(opts);
                break;
            case "edge":
                opts.Channel = "msedge";
                Browser = await Playwright.Chromium.LaunchAsync(opts);
                break;
            case "firefox":
                Browser = await Playwright.Firefox.LaunchAsync(opts);
                break;
            default:
                opts.Channel = "chrome";
                Browser = await Playwright.Chromium.LaunchAsync(opts);
                break;
        }
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1366, Height = 768 },
            RecordVideoDir = Path.Combine(artifactsRoot, "videos")
        });
        Page = await Context.NewPageAsync();
        Page.Dialog += async (_, dialog) =>
        {
            if (dialog.Type == "alert" || dialog.Type == "confirm")
            {
                await dialog.AcceptAsync();
            }
        };
    }

    public async Task NavigateBaseAsync()
    {
        if (Page == null) throw new InvalidOperationException();
        await Page.GotoAsync(Config.BaseUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 60000 });
    }

    public async ValueTask DisposeAsync()
    {
        if (Context != null)
        {
            await Context.CloseAsync();
        }
        if (Browser != null)
        {
            await Browser.CloseAsync();
        }
        Playwright?.Dispose();
    }
}
