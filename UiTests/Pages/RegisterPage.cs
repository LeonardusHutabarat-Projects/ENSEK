using Microsoft.Playwright;

namespace UiTests.Pages
{

    /// <summary>
    /// 
    /// This class represents the user registration page and 
    /// provides methods to automate user registration via Playwright UI tests.
    /// 
    /// </summary>

    public class RegisterPage
    {
        private readonly IPage _page;

        public ILocator EmailInput => _page.Locator("#Email");
        public ILocator PasswordInput => _page.Locator("#Password");
        public ILocator ConfirmPasswordInput => _page.Locator("#ConfirmPassword");
        public ILocator RegisterButton => _page.Locator("input[value='Register']");

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