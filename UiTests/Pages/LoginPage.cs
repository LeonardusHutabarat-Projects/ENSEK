using Microsoft.Playwright;

namespace UiTests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        // Locators for LoginPage
        public ILocator RegisterLink => _page.Locator("#registerLink");

        // Constructor
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
