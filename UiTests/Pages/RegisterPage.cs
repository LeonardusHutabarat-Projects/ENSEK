using Microsoft.Playwright;

namespace UiTests.Pages
{
    public class RegisterPage
    {
        private readonly IPage _page;

        // Locators for RegisterPage
        public ILocator EmailInput => _page.Locator("#Email");
        public ILocator PasswordInput => _page.Locator("#Password");
        public ILocator ConfirmPasswordInput => _page.Locator("#ConfirmPassword");
        public ILocator RegisterButton => _page.Locator("input[value='Register']"); // For button with value='Register'

        // Constructor
        public RegisterPage(IPage page)
        {
            _page = page;
        }

        public async Task RegisterNewUserAsync(string email, string password)
        {
            await EmailInput.FillAsync(email);
            await PasswordInput.FillAsync(password);
            await ConfirmPasswordInput.FillAsync(password);
            await RegisterButton.ClickAsync();
        }
    }
}

