using Helpers;
using Microsoft.Playwright.NUnit;
using UiTests.Pages;

namespace UiTests.Tests
{

    /// <summary>
    /// 
    /// This UI test is for  verifying that a new user can register successfully
    /// on the application.
    /// 
    /// Steps:
    /// 1. Navigate to the home page using the TestDataHelper URL.
    /// 2. Click the register link from the login page.
    /// 3. Assert that the current page URL contains the account register path.
    /// 4. Fill in and submit the registration form with test email and password
    ///    from TestDataHelper.
    /// 5. Check if an error message is displayed. 
    ///    If found, log the error text and fail the test.
    /// 
    /// Purpose:
    /// Ensures that user registration workflow functions correctly and that no error messages
    /// are returned when registering with valid test data.
    /// 
    /// </summary>

    [TestFixture]
    public class NewUserRegistration : PageTest
    {
        [Test]
        public async Task RegisterNewUser_ShouldRegisterNewUser()
        {
            await Page.GotoAsync(TestDataHelper.HomeUrl);

            var loginPage = new LoginPage(Page);
            await loginPage.ClickRegisterLinkAsync();

            Assert.That(Page.Url, Does.Contain(TestDataHelper.AccountRegisterHref));

            var registerPage = new RegisterPage(Page);
            string testEmail = TestDataHelper.TestEmail;
            string testPassword = TestDataHelper.TestPassword;

            await registerPage.RegisterNewUserAsync(testEmail, testPassword);

            var errorLocator = Page.Locator(".text-danger");
            if (await errorLocator.IsVisibleAsync())
            {
                string errorText = await errorLocator.InnerTextAsync();
                Console.WriteLine(errorText);
                Assert.Fail($"Registration failed with error");
            }
        }
    }
}