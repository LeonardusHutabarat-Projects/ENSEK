using Helpers;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace ENSEKAutomationTests.ApiTests
{

    /// <summary>
    /// 
    /// This API test is for verifying the endpoint of /ENSEK/energy.
    ///  
    /// This test class covers two main scenarios:
    /// (1) GET request to retrieve a list of available energy types:
    ///     - Endpoint: GET /energy
    ///     - Validates the API responds with 200 OK and
    ///       returns a properly formatted JSON list of energy types.
    ///     - Pretty-prints the JSON content for debugging and diagnostics.
    ///     - Fails if the response returns any unexpected HTTP status code.
    /// 
    /// (2) PUT request to purchase a specified quantity of an energy unit:
    ///     - Endpoint: PUT /buy/{energyId}/{quantity}
    ///     - Attempts to purchase a valid energy type with a provided ID and quantity.
    ///     - Expects a 200 OK response with a confirmation message containing
    ///       "You have purchased".
    ///     - Handles errors and asserts failure for invalid input
    ///       (400 Bad Request) or any unexpected status code.
    ///     - Exception handling ensures unexpected runtime issues are caught
    ///       and reported as test failures.
    /// 
    /// The RestClient instance is initialised before each test and disposed after execution.
    /// These automated checks provide coverage for critical ENSEK energy API functionality: 
    /// retrieving available energy types and completing energy purchases.
    /// 
    /// </summary>

    [TestFixture]
    public class EnergyApiTests
    {
        private RestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new RestClient(TestDataHelper.ApiUrl);
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public async Task GetDetailsOnEnergyType_ShouldGiveList()
        {
            var request = new RestRequest(TestDataHelper.EnergyHref, Method.Get);
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
            var request = new RestRequest($"{TestDataHelper.BuyEnergyHref}/{energyId}/{quantity}", Method.Put);

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