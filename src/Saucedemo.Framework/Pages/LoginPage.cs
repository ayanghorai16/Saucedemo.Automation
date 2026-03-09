using System.Threading.Tasks;
using Microsoft.Playwright;
using Saucedemo.Framework.Core.Selectors;

namespace Saucedemo.Framework.Pages;

public sealed class LoginPage
{
    private readonly IPage _page;
    public LoginPage(IPage page) => _page = page;

    public ILocator Username => _page.Locator(UiMap.Login.UsernameInput);
    public ILocator Password => _page.Locator(UiMap.Login.PasswordInput);
    public ILocator LoginButton => _page.Locator(UiMap.Login.LoginButton);
    public ILocator ErrorMessage => _page.Locator(UiMap.Login.ErrorMessage);

    public async Task LoginAsync(string username, string password)
    {
        await Username.FillAsync(username);
        await Password.FillAsync(password);
        await LoginButton.ClickAsync();
    }
}
