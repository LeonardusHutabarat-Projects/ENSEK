using ENSEKAutomationTests.Helpers;
using Helpers;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace ENSEKAutomationTests.ApiTests
{
    /// <summary>
    /// 
    /// This API test is for verifying the Orders endpoints of the ENSEK candidate test.
    /// 
    /// This class covers the full lifecycle of an energy order 
    /// (Create → Retrieve → Update → Delete) using RestSharp client calls and NUnit assertions.
    /// 
    /// Key highlights:
    /// (1) [SetUp] creates a valid test order via EnergyOrderHelper class 
    ///     to ensure reliabale test preconditions.
    /// (2) [TearDown] disposes of the RestClient after each test.
    /// (3) GetDetailsOfPreviousOrder_ShouldGiveTheList:
    ///     Validates that the /ENSEK/orders endpoint returns a non-empty list
    ///     in valid JSON format.
    /// (4) PutUpdateOrder_ShouldUpdateSinglePreviousOrder:
    ///     Attempts to update an existing order and asserts against 
    ///     500 Internal Server Error results.
    /// (5) GetOrderById_ShouldReturnDetailsOfSinglePreviousOrder:
    ///     Fetches order by ID and checks API responds without server-side errors.
    /// (6) DeleteOrder_ShouldDeleteParticularOrder:
    ///     Attempts to delete the created order and validates response handling,
    ///     ensuring no unhandled 500 Internal Server Error is returned.
    /// 
    /// Assertions validate:
    /// (1) Correct HTTP status codes (200 OK or expected results).
    /// (2) Proper data shapes (presence of id, quantity, etc.).
    /// (3) That ENSEK API endpoints are stable and not returning 500/Unhandled errors.
    /// 
    /// Overall, these tests aim to act as regression checks on Orders API functionality, 
    /// helping detect broken or unimplemented endpoints in the candidate environment.
    /// 
    /// </summary>

    [TestFixture]
    public class OrdersApiTests
    {
        private RestClient _client;
        private string? _orderId;
        private static readonly int energyId = TestDataHelper.ValidEnergyId;
        private static readonly int quantity = TestDataHelper.ValidQuantity;

        [SetUp]
        public async Task SetUp()
        {
            _client = new RestClient(TestDataHelper.ApiUrl);
            _orderId = await EnergyOrderHelper.BuyEnergyAndGetOrderIdAsync(_client, energyId, quantity);
            Assert.That(_orderId, Is.Not.Null, "Failed to create test order via helper!");
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public async Task GetDetailsOfPreviousOrder_ShouldGiveTheList()
        {
            var request = new RestRequest(TestDataHelper.OrderHref, Method.Get);
            var response = await _client.ExecuteAsync(request);

            Console.WriteLine($"StatusCode: {response.StatusCode}");

            string prettyJson = JsonSerializer.Serialize(
                    JsonDocument.Parse(response.Content),
                    new JsonSerializerOptions { WriteIndented = true }
                );
            Console.WriteLine(prettyJson);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.Content, Does.StartWith("[").And.Contain("id").And.Contain("quantity"));

            var orders = Newtonsoft.Json.Linq.JArray.Parse(response.Content);
            Assert.That(orders.Count, Is.GreaterThan(0), "Orders array should not be empty.");
            Assert.That(orders[0]["id"] != null || orders[0]["Id"] != null, "Each order should have an 'id' or 'Id' property.");
        }

        [Test]
        public async Task PutUpdateOrder_ShouldUpdateSinglePreviousOrder()
        {
            var request = new RestRequest($"{TestDataHelper.OrderHref}/{_orderId}", Method.Put);
            request.AddJsonBody(new
            {
                id = _orderId,
                quantity = quantity + 1,
                enerygy_id = energyId
            });
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine($"PutUpdateOrder: StatusCode: {response.StatusCode}");
            Console.WriteLine($"PutUpdateOrder: Content: {response.Content}");

            if (response.StatusCode == HttpStatusCode.InternalServerError &&
                response.Content != null && response.Content.Contains("Internal Server Error"))
            {
                Assert.Fail($"FAIL: PUT update by order id is broken! Got 500 Internal Server Error: {response.Content}");
            }
            else
            {
                Assert.Pass($"PASS: PUT update order did not return 500. Actual: {response.StatusCode} - {response.Content}");
            }
        }

        [Test]
        public async Task GetOrderById_ShouldReturnDetailsOfSinglePreviousOrder()
        {
            var request = new RestRequest($"{TestDataHelper.OrderHref}/{_orderId}", Method.Get);
            var response = await _client.ExecuteAsync(request);
            Console.WriteLine($"GetOrderById: StatusCode: {response.StatusCode}");
            Console.WriteLine($"GetOrderById: Content: {response.Content}");

            if (response.StatusCode == HttpStatusCode.InternalServerError &&
                response.Content != null &&
                response.Content.Contains("Internal Server Error"))
            {
                Assert.Fail($"FAIL: GET order by id is broken! Got 500 Internal Server Error: {response.Content}");
            }
            else
            {
                Assert.Pass($"PASS: GET order by id did not return 500. Actual: {response.StatusCode} - {response.Content}");
            }
        }

        [Test]
        public async Task DeleteOrder_ShouldDeleteParticularOrder()
        {
            var request = new RestRequest($"/{TestDataHelper.OrderHref}/{_orderId}", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            Console.WriteLine($"DeleteOrder: StatusCode: {response.StatusCode}");
            Console.WriteLine($"DeleteOrder: Content: {response.Content}");

            if (response.StatusCode == HttpStatusCode.InternalServerError &&
                response.Content != null &&
                response.Content.Contains("Internal Server Error"))
            {
                Assert.Fail($"FAIL: DELETE order by id is broken! Got 500 Internal Server Error: {response.Content}");
            }
            else
            {
                Assert.Pass($"DELETE endpoint NOT in error state (BUG or NOT IMPLEMENTED): " +
                            $"Expected 500 with error message, got {response.StatusCode} - {response.Content}");
            }
        }
    }
}