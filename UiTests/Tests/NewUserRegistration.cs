using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using UiTests.Pages;
using Helpers;

namespace UiTests.Tests
{
    [TestFixture]
    public class NewUserRegistration : PageTest
    {
        [Test]
        public async Task NewUserCanRegisterSuccessfully()
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

            //Assert.That(Page.Url, Does.Contain("/Account/Register"), "Should attempt to register");
            
        }
    }
}

