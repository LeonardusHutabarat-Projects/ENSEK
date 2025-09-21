using Helpers;
using Microsoft.Playwright.NUnit;

namespace UiTests.Tests
{

    /// <summary>
    /// 
    /// This UI test is for verifying the availability of the Sell Energy page.
    /// Navigates to the home page, clicks the Sell Energy link, and checks
    /// whether the page is accessible or under maintenance. The test fails
    /// if a maintenance image is displayed, otherwise it passes.
    /// 
    /// </summary>

    [TestFixture]
    public class SellEnergy : PageTest
    {
        [Test]  
        public async Task SellEnergy_ShouldBeAbleToSellEnergy()
        {
            await Page.GotoAsync(TestDataHelper.HomeUrl);
            await Page.ClickAsync($"a[href='{TestDataHelper.SellEnergyHref}']");
            await Expect(Page).ToHaveURLAsync(TestDataHelper.SellEnergyUrl);
            var maintenanceImg = Page.Locator($"img[src='{TestDataHelper.MaintenanceImageSrc}']");

            bool isMaintenancePresent = await Page.Locator("img[src*='maintenance-1151312_960_720.png']").CountAsync() > 0;

            if (isMaintenancePresent)
            {
                Assert.Fail("Sell Energy page is not available.");
            }
            else
            {
                Assert.Pass("Sell Energy page is available");
            }
        }
    }
}