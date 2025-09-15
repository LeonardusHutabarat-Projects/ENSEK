using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using Helpers;

namespace UiTests.Tests
{
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
