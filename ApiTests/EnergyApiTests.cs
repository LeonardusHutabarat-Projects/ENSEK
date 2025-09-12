using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Helpers;

namespace ENSEKAutomationTests.ApiTests
{
    [TestFixture]
    public class EnergyApiTests
    {
        private RestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new RestClient("https://qacandidatetest.ensek.io");
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public async Task GetDetailsOnEnergyType_ShouldGiveList()
        {
            var request = new RestRequest("/ENSEK/energy", Method.Get);
            RestResponse? response = null;
            response = await _client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string prettyJson = JsonSerializer.Serialize(
                    JsonDocument.Parse(response.Content),
                    new JsonSerializerOptions { WriteIndented = true }
                );
                Console.WriteLine(prettyJson);
                Assert.Pass("Get Details on Energy Type successful: 200 OK");
            }
            else
            {
                Assert.Fail($"Get Details on Energy Type failed. Unexpected status code: {(int)response.StatusCode} {response.StatusCode}");
            }
        }

        [Test]
        public async Task PurchaseEnergyUnits_ShouldPurchaseUnits()
        {
            int energyId = TestDataHelper.ValidEnergyId;
            int quantity = TestDataHelper.ValidQuantity;
            var request = new RestRequest($"/ENSEK/buy/{energyId}/{quantity}", Method.Put);

            RestResponse? response = null;
            try
            {
                response = await _client.ExecuteAsync(request);
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"Content: {response.Content}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Assert.That(response.Content, Does.Contain("You have purchased"), "Expected confirmation message not found.");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Assert.Fail("Bad Request: Check if the ID/quantity is valid.");
                }
                else
                {
                    Assert.Fail($"Unexpected response: {response.StatusCode} - {response.Content}");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception thrown during API call: {ex.Message}");
            }
        }
    }
}
