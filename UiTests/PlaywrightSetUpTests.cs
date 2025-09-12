using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace UiTests
{
    [TestFixture]
    public class PlaywrightSetupTests
    {
        [Test]
        public async Task DownloadBrowsers()
        {
            using var playwright = await Playwright.CreateAsync();
            
            Assert.That(playwright, Is.Not.Null);
        }
    }
}

