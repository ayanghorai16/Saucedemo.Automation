// The System and NUnit namespaces are used for basic functionality and testing framework features.
// The Allure namespaces are used for reporting test steps and attachments to Allure.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Allure.NUnit.Attributes;
using Allure.NUnit;
using Allure.Commons;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Saucedemo.Framework.Core;

namespace Saucedemo.Tests;

[Parallelizable(ParallelScope.Self)]
public abstract class BaseTest
{
    protected PlaywrightDriver Driver = new();
    protected string TestName => TestContext.CurrentContext.Test.Name;
    private string? _lastScreenshot;
    private static bool _browsersInstalled = false;
    private static readonly object _lock = new();

    [OneTimeSetUp]
    public void OneTime()
    {
        var root = Config.ArtifactsRoot;
        if (Directory.Exists(root)) Directory.Delete(root, true);
        Directory.CreateDirectory(root);
        lock (_lock)
        {
            if (!_browsersInstalled)
            {
                try
                {
                    Microsoft.Playwright.Program.Main(new[] { "install" });
                }
                catch { }
                _browsersInstalled = true;
            }
        }
    }

    [SetUp]
    public async Task SetUp()
    {
        await Driver.InitializeAsync();
        await Driver.NavigateBaseAsync();
    }

    protected async Task Step(string name)
    {
        if (Driver.Page == null) throw new InvalidOperationException();
        _lastScreenshot = await StepHelper.Capture(Driver.Page, TestName, name);
        
        var uuid = Guid.NewGuid().ToString();
        // wrap all Allure interaction in a broad try/catch so that any
        // unexpected exception (null parentUuid, missing dictionary entry,
        // etc.) cannot escape and fail the test. The step and attachment are
        // only for reporting purposes.
        try
        {
            AllureLifecycle.Instance.StartStep(uuid, new StepResult { name = name, status = Status.passed });
            try
            {
                AllureLifecycle.Instance.AddAttachment(name, "image/png", _lastScreenshot);
            }
            catch
            {
                // ignore attachment failures
            }
        }
        catch
        {
            // ignore start-step failures
        }
        finally
        {
            try
            {
                AllureLifecycle.Instance.StopStep(uuid);
            }
            catch
            {
                // ignore stop-step failures
            }
        }
    }

    private async Task ExecuteStep(string name)
    {
        await Step(name);
    }

    [TearDown]
    public async Task TearDown()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome.Status;
        if (outcome == TestStatus.Failed && Driver.Page != null)
        {
            try
            {
                var path = await StepHelper.Capture(Driver.Page, TestName, "FAIL_LAST_VIEW");
                AllureLifecycle.Instance.AddAttachment("FAIL_LAST_VIEW", "image/png", path);
            }
            catch
            {
            }
        }
        await Driver.DisposeAsync();
    }
}
