using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using NUnit.Framework;
using Saucedemo.Framework.Core;
using Saucedemo.Framework.Pages;
using Saucedemo.Framework.Utils;

namespace Saucedemo.Tests;

[AllureNUnit]
[AllureSuite("Login")]
public class LoginTests : BaseTest
{
    static IEnumerable<TestCaseData> Users()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "TestData", "users.csv");
        var full = Path.GetFullPath(path);
        foreach (var u in CsvData.LoadUsers(full).Where(x => string.Equals(x.Execute, "Yes", StringComparison.OrdinalIgnoreCase)))
        {
            yield return new TestCaseData(u.Username).SetName($"Login_{u.Username}");
        }
    }

    [Test]
    [TestCaseSource(nameof(Users))]
    public async Task ValidateUserLogin(string username)
    {
        if (Driver.Page == null) Assert.Fail();
        var page = Driver.Page;
        var login = new LoginPage(page);
        await Step($"Open {Config.BaseUrl}");
        await login.LoginAsync(username, "secret_sauce");
        await Step($"Submit credentials for {username}");
        var products = new ProductsPage(page);
        string message;
        bool success;
        try
        {
            await products.Title.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });
            success = true;
        }
        catch
        {
            success = false;
        }
        if (success)
        {
            message = "Login successful";
            await Step("Products visible");
            var menu = new MenuBarPage(page);
            await menu.OpenAsync();
            await Step("Menu open");
            await menu.LogoutAsync();
            await Step("Logout");
        }
        else
        {
            string errorText = "";
            if (await login.ErrorMessage.IsVisibleAsync())
            {
                errorText = await login.ErrorMessage.InnerTextAsync();
            }
            message = string.IsNullOrWhiteSpace(errorText) ? "Login failed: Unknown error" : $"Login failed: {errorText}";
            await Step(string.IsNullOrWhiteSpace(errorText) ? "No error displayed" : "Error displayed");
        }
        TestContext.WriteLine(message);
        StepHelper.AttachText("final-message", message);
    }
}
