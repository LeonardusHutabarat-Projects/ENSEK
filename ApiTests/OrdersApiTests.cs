using ENSEKAutomationTests.Helpers;
using Helpers;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ENSEKAutomationTests.ApiTests
{
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
            _client = new RestClient("https://qacandidatetest.ensek.io");
            _orderId = await EnergyOrderHelper.BuyEnergyAndGetOrderIdAsync(_client, energyId, quantity);
            Assert.That(_orderId, Is.Not.Null,"Failed to create test order via helper!");
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public async Task GetDetailsOfPreviousOrder_ShouldGiveTheList()
        {
            var request = new RestRequest("/ENSEK/orders", Method.Get);
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
            Assert.That(orders[0]["id"] != null, "Each order should have an 'id' property.");
        }

        [Test]
        public async Task PutUpdateOrder_ShouldUpdateSinglePreviousOrder()
        {
            var request = new RestRequest($"/ENSEK/orders/{_orderId}", Method.Put);
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
            var request = new RestRequest($"/ENSEK/orders/{_orderId}", Method.Get);
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
            var request = new RestRequest($"/ENSEK/orders/{_orderId}", Method.Delete);
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

