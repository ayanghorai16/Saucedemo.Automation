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

    [OneTimeSetUp]
    public void OneTime()
    {
        Directory.CreateDirectory(Config.ArtifactsRoot);
    }

    [SetUp]
    public async Task SetUp()
    {
        await Driver.InitializeAsync();
        await Driver.NavigateBaseAsync();
    }

    protected async Task<string> Step(string name)
    {
        if (Driver.Page == null) throw new InvalidOperationException();
        _lastScreenshot = await StepHelper.Capture(Driver.Page, TestName, name);
        return _lastScreenshot;
    }

    [TearDown]
    public async Task TearDown()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome.Status;
        if (outcome == TestStatus.Failed && _lastScreenshot != null && File.Exists(_lastScreenshot))
        {
            try
            {
                var bytes = await File.ReadAllBytesAsync(_lastScreenshot);
                AllureLifecycle.Instance.AddAttachment("failure", "image/png", bytes, "png");
            }
            catch
            {
            }
        }
        await Driver.DisposeAsync();
    }
}
