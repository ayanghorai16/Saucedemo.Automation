using Microsoft.Playwright;

namespace Saucedemo.Framework.Core.Selectors;

public static class UiMap
{
    public static class Login
    {
        public static string UsernameInput => "//input[@data-test='username']";
        public static string PasswordInput => "//input[@data-test='password']";
        public static string LoginButton => "//input[@data-test='login-button']";
        public static string ErrorMessage => "//h3[@data-test='error']";
    }
    public static class Menu
    {
        public static string BurgerButton => "//button[@id='react-burger-menu-btn']";
        public static string CloseButton => "//button[@id='react-burger-cross-btn']";
        public static string AllItems => "//a[@id='inventory_sidebar_link']";
        public static string About => "//a[@id='about_sidebar_link']";
        public static string Logout => "//a[@id='logout_sidebar_link']";
        public static string Reset => "//a[@id='reset_sidebar_link']";
        public static string SidebarContainer => "//div[contains(@class,'bm-menu-wrap')]";
    }
    public static class Products
    {
        public static string Title => "//span[@class='title' and text()='Products']";
        public static string InventoryContainer => "//*[@data-test='inventory-container']";
    }
}
