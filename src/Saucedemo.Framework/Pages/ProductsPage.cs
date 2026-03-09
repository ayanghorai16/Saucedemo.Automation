using System.Threading.Tasks;
using Microsoft.Playwright;
using Saucedemo.Framework.Core.Selectors;

namespace Saucedemo.Framework.Pages;

public sealed class ProductsPage
{
    private readonly IPage _page;
    public ProductsPage(IPage page) => _page = page;

    public ILocator Title => _page.Locator(UiMap.Products.Title);
    public ILocator Inventory => _page.Locator(UiMap.Products.InventoryContainer);

    public async Task<bool> IsLoadedAsync()
    {
        return await Title.IsVisibleAsync() && await Inventory.IsVisibleAsync();
    }
}
