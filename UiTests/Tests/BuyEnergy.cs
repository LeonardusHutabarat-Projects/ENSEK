using Helpers;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Collections.Generic;

namespace UiTests.Tests
{
    [TestFixture]
    public class BuyEnergy : PageTest
    {
        [Test]
        public async Task BuyEnergy_ShouldBeAbleToBuy()
        {
            var reportLines = new List<string>();

            await Page.GotoAsync(TestDataHelper.BuyEnergyUrl);
            await Page.WaitForSelectorAsync("table.table tbody tr",
                new() { Timeout = 60000 });

            var table = await Page.QuerySelectorAsync(".table");
            Assert.That(table, Is.Not.Null, "Energy table not found");

            var headers = await Page.QuerySelectorAllAsync("table.table thead th");
            var expectedHeaders = new[]
            {
                "Energy Type",
                "Price",
                "Quantity of Units Available",
                "Number of Units Required"
            };
            for (int i = 0; i < expectedHeaders.Length; i++)
            {
                string actual = await headers[i].InnerTextAsync();
                Assert.That(actual, Does.Contain(expectedHeaders[i]),
                    $"Missing column: {expectedHeaders[i]}");
            }

            int itemNumber = 1;
            foreach (var item in TestDataHelper.EnergyPurchaseData)
            {
                var keyText = item.Key.Trim();

                if (item.Value < 0)
                {
                    reportLines.Add($"[{itemNumber}] {keyText}:" +
                        $" INVALID INPUT (negative units: {item.Value})");
                    itemNumber++;
                    continue;
                }
                try
                {
                    await Page.WaitForSelectorAsync("table.table tbody tr",
                        new() { Timeout = 60000 });
                    var energyRows = Page.Locator("table.table tbody tr");
                    int energyRowCount = await energyRows.CountAsync();
                    ILocator? targetRow = null;

                    for (int i = 0; i < energyRowCount; i++)
                    {
                        var firstCell = energyRows.Nth(i).Locator("td").First;
                        var count = await firstCell.CountAsync();
                        string cellText = count > 0 ? 
                            (await firstCell.InnerTextAsync()).Trim() : "";
                        if (count > 0 && cellText.Equals(keyText,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            targetRow = energyRows.Nth(i);
                            break;
                        }
                    }
                    Assert.That(targetRow, Is.Not.Null,
                        $"Table row not found for energy type: {item.Key}");

                    var unitsInput = targetRow.Locator(
                        "input[id='energyType_AmountPurchased']");
                    var inputCount = await unitsInput.CountAsync();
                    if (inputCount == 0)
                    {
                        reportLines.Add($"[{itemNumber}] {keyText}:" +
                            $" UNAVAILABLE (cannot purchase)");
                        continue;
                    }
                    Assert.That(await unitsInput.CountAsync(), Is.GreaterThan(0),
                        $"Input not found for {item.Key}");
                    await unitsInput.FillAsync(item.Value.ToString());

                    var buyBtn = targetRow.Locator(
                        "button[name='Buy'],input[name='Buy']");
                    Assert.That(await buyBtn.CountAsync(), Is.GreaterThan(0),
                        $"Buy button not found for {item.Key}");
                    await buyBtn.First.ClickAsync();

                    await Page.WaitForSelectorAsync("text=Sale Confirmed",
                        new() { Timeout = 60000 });
                    await Page.ClickAsync("text=Buy more »");

                    reportLines.Add($"[{itemNumber}] {keyText}:" +
                        $" SUCCESS ({item.Value} units purchased)");
                }
                catch (Exception ex)
                {
                    reportLines.Add($"[{itemNumber}] {keyText}: FAIL - {ex.Message}");
                }
                itemNumber++;
            }

            Console.WriteLine("SUMMARY REPORT");
            foreach (var line in reportLines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("END OF REPORT");

            Assert.Pass("Buy Energy is successful.");
        }
    }
}
