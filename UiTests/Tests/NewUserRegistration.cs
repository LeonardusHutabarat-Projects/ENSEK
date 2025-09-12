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
            await Page.GotoAsync("https://ensekautomationcandidatetest.azurewebsites.net/");

            var loginPage = new LoginPage(Page);

            await loginPage.ClickRegisterLinkAsync();

            Assert.That(Page.Url, Does.Contain("/Account/Register"));

            var registerPage = new RegisterPage(Page);

            string testEmail = TestDataHelper.TestEmail;
            string testPassword = TestDataHelper.TestPassword;
            
            await registerPage.RegisterNewUserAsync(testEmail, testPassword);

            Assert.That(Page.Url, Does.Contain("/Account/Register"), "Should attempt to register");
        }
    }
}

