using Microsoft.Playwright;

namespace UiTests.Pages
{

    /// <summary>
    /// 
    /// This class represents the Login Page of the application under test.
    /// Provides page object methods and locators to interact with elements
    /// on the login screen, such as navigating to the registration page.
    /// 
    /// </summary>

    public class LoginPage
    {
        private readonly IPage _page;

        public ILocator RegisterLink => _page.Locator("#registerLink");

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task ClickRegisterLinkAsync()
        {
            await RegisterLink.ClickAsync();
        }
    }
}