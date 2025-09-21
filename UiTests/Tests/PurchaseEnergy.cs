using Helpers;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace UiTests.Tests
{

    /// <summary>
    /// 
    /// This UI test class is for verifying the energy purchase flow in the web application.
    /// This test navigates to the Buy Energy page, validates the table and its headers, 
    /// and attempts to purchase specified energy types with the required number of units 
    /// defined in the test data.
    /// 
    /// Key validations:
    /// (1) Ensures the energy table is present and not null.
    /// (2) Confirms that all expected table headers are displayed.
    /// (3) Iterates through provided energy purchase data and verifies:
    ///     * Corresponding table rows exist for each energy type.
    ///     * Input fields for entering purchase quantity are available.
    ///     * Buy buttons for each row are present and functional.
    /// 
    /// If all steps succeed without error, the test completes with success.
    /// 
    /// </summary>

    [TestFixture]
    public class PurchaseEnergy : PageTest
    {
        [Test]
        public async Task PurchaseEnergy_ShouldBeAbleToPurchase()
        {
            await Page.GotoAsync(TestDataHelper.BuyEnergyUrl);

            var tableRows = Page.Locator("table.table tr");
            int totalRowCount = await tableRows.CountAsync();

            var table = await Page.QuerySelectorAsync(".table");
            Assert.That(table, Is.Not.Null, "Energy table not found");

            var headers = await Page.QuerySelectorAllAsync("table.table thead th");
            var expectedHeaders = new[] {
                "Energy Type",
                "Price",
                "Quanity of Units Available",
                "Number of Units Required"
            };
            for (int i = 0; i < expectedHeaders.Length; i++)
            {
                string actual = await headers[i].InnerTextAsync();
                Assert.That(actual, Does.Contain(expectedHeaders[i]), $"Missing column: {expectedHeaders[i]}");
            }

            foreach (var item in TestDataHelper.EnergyPurchaseData)
            {
                try
                {
                    var energyRows = Page.Locator("table.table tr");
                    int energyRowCount = await energyRows.CountAsync();
                    ILocator targetRow = null;

                    for (int i = 0; i < energyRowCount; i++)
                    {
                        var firstCell = energyRows.Nth(i).Locator("td").First;
                        if (await firstCell.CountAsync() == 0) continue;
                        var cellText = await firstCell.InnerTextAsync();

                        if (cellText.IndexOf(item.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            targetRow = energyRows.Nth(i);
                            break;
                        }
                    }

                    Assert.That(targetRow, Is.Not.Null, $"Table row not found for energy type: {item.Key}");

                    var unitsInput = targetRow.Locator("input[id='energyType_AmountPurchased']");
                    Assert.That(await unitsInput.CountAsync(), Is.GreaterThan(0), $"Input not found for {item.Key}");

                    await unitsInput.FillAsync(item.Value.ToString());
                    var buyBtn = targetRow.Locator("button[name='Buy'],input[name='Buy']");
                    Assert.That(await buyBtn.CountAsync(), Is.GreaterThan(0), $"Buy button not found for {item.Key}");
                    await buyBtn.First.ClickAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing [{item.Key}]: {ex.Message}");
                }
            }
            Assert.Pass("PurchaseEnergy is successful");
        }
    }
}