using System.Threading.Tasks;
using Microsoft.Playwright;
using Saucedemo.Framework.Core.Selectors;

namespace Saucedemo.Framework.Pages;

public sealed class MenuBarPage
{
    private readonly IPage _page;
    public MenuBarPage(IPage page) => _page = page;

    public ILocator BurgerButton => _page.Locator(UiMap.Menu.BurgerButton);
    public ILocator CloseButton => _page.Locator(UiMap.Menu.CloseButton);
    public ILocator Sidebar => _page.Locator(UiMap.Menu.SidebarContainer);
    public ILocator AllItems => _page.Locator(UiMap.Menu.AllItems);
    public ILocator About => _page.Locator(UiMap.Menu.About);
    public ILocator Logout => _page.Locator(UiMap.Menu.Logout);
    public ILocator Reset => _page.Locator(UiMap.Menu.Reset);

    public async Task OpenAsync()
    {
        if (!await Sidebar.IsVisibleAsync())
        {
            await BurgerButton.ClickAsync();
            await Sidebar.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await CloseButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        }
    }
    public async Task CloseAsync()
    {
        if (await Sidebar.IsVisibleAsync())
        {
            await CloseButton.DispatchEventAsync("click");
            await BurgerButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        }
    }
    public async Task NavigateAllItemsAsync()
    {
        await OpenAsync();
        await AllItems.DispatchEventAsync("click");
        await TitleCheck();
    }
    public async Task NavigateAboutAsync()
    {
        await OpenAsync();
        await About.DispatchEventAsync("click");
    }
    public async Task LogoutAsync()
    {
        await OpenAsync();
        await Logout.DispatchEventAsync("click");
    }
    public async Task ResetAppStateAsync()
    {
        await OpenAsync();
        await Reset.DispatchEventAsync("click");
        await BurgerButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    }
    private async Task TitleCheck()
    {
        var title = _page.Locator(UiMap.Products.Title);
        await title.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
    }
}
