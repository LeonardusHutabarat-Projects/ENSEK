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
            Page.SetDefaultTimeout(0);

            await Page.GotoAsync(TestDataHelper.HomeUrl);

            var loginPage = new LoginPage(Page);
            await loginPage.ClickRegisterLinkAsync();

            Assert.That(Page.Url, Does.Contain(TestDataHelper.
                AccountRegisterHref));

            var registerPage = new RegisterPage(Page);
            string testEmail = TestDataHelper.TestEmail;
            string testPassword = TestDataHelper.TestPassword;

            await registerPage.RegisterNewUserAsync(testEmail, testPassword);

            var h1Locator = Page.Locator("h1.text-danger");
            var h2Locator = Page.Locator("h2.text-danger");
            var h3Locator = Page.Locator("h3");

            string h1Text = await h1Locator.CountAsync() > 0
                ? await h1Locator.First.InnerTextAsync()
                : string.Empty;
            string h2Text = await h2Locator.CountAsync() > 0
                ? await h2Locator.First.InnerTextAsync()
                : string.Empty;
            string h3Text = await h3Locator.CountAsync() > 0
                ? await h3Locator.First.InnerTextAsync()
                : string.Empty;

            if (!string.IsNullOrWhiteSpace(h1Text) ||
                !string.IsNullOrWhiteSpace(h2Text) ||
                !string.IsNullOrWhiteSpace(h3Text))
            {
                string fullErrorText = $"{h1Text}\n{h2Text}\n{h3Text}";
                string[] errorParts = fullErrorText.Split('.');
                string formattedError = string.Join("\n",
                    errorParts.Select(p => p.Trim()).Where(p =>
                    !string.IsNullOrEmpty(p)));
                Assert.That(fullErrorText, Does.Contain("Error"));
                Assert.That(fullErrorText, Does.Contain(
                    "An error occurred while"));
                Assert.That(fullErrorText, Does.Contain(
                    "A network-related or"));

                Assert.Fail("New User Registration failed." +
                    " Messages as follows:\n" + formattedError);
            }
        }

    }
}