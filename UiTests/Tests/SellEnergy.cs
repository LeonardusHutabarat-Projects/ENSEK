//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using Helpers;

namespace UiTests.Tests
{
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
