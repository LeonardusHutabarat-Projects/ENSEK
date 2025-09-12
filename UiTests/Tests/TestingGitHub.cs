using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;

namespace UiTests.Tests
{
    [TestFixture]
    public class TestingGitHub : PageTest
    {
        [Test]  
        public async Task TestingGitHub_ShouldPrint()
        {
            await Page.GotoAsync("https://ensekautomationcandidatetest.azurewebsites.net/Energy/Buy");
            Console.WriteLine("Hello GitHub");
            Console.WriteLine("Using new GitHub account");
            Console.WriteLine("My name is Leo");
            Console.WriteLine("My last name is Hutabarat");
        }
    }
}
