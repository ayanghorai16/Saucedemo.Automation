using System.Threading.Tasks;
using NUnit.Framework;
using Saucedemo.Framework.Core;
using Saucedemo.Framework.Pages;

namespace Saucedemo.Tests;

[TestFixture]
public class MenuBarTests : BaseTest
{
    [Test]
    public async Task Menu_Open_Close_And_Navigate()
    {
        if (Driver.Page == null) Assert.Fail();
        var page = Driver.Page;
        var login = new LoginPage(page);
        await Step("Open site");
        await login.LoginAsync("standard_user", "secret_sauce");
        await Step("Login standard_user");
        var products = new ProductsPage(page);
        Assert.That(await products.IsLoadedAsync(), Is.True, "Products should be visible");
        var menu = new MenuBarPage(page);
        await menu.OpenAsync();
        await Step("Open menu");
        await menu.NavigateAllItemsAsync();
        await Step("All Items");
        await menu.OpenAsync();
        await menu.ResetAppStateAsync();
        await Step("Reset App State");
        await menu.OpenAsync();
        await menu.LogoutAsync();
        await Step("Logout");
    }
}
