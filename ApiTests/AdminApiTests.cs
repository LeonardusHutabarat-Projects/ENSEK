using Helpers;
using RestSharp;
using System.Net;

namespace ENSEKAutomationTests.ApiTests
{

    /// <summary>
    /// 
    /// This API test is for verifying the functionality of the Admin Reset API endpoint
    /// (/ENSEK/reset).
    /// 
    /// The test sends a POST request to reset the application’s test data and validates 
    /// the response code:
    /// - 200 OK confirms a successful reset
    /// - 401 Unauthorized indicates lack of authorization
    /// - Any other status codes are treated as unexpected failures
    /// 
    /// This test ensures the reset functionality works correctly and provides
    /// clear feedback in both success and error scenarios.
    /// 
    /// </summary>

    [TestFixture]
    public class AdminApiTests
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
        public async Task ResetTestData_ShouldResetTestData()
        {
            var request = new RestRequest("/ENSEK/reset", Method.Post);

            var response = await _client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Assert.Pass("Reset successful: 200 OK");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Assert.Fail("Reset failed: 401 Unauthorized");
            }
            else
            {
                Assert.Fail($"Reset failed. Unexpected status code: {(int)response.StatusCode} {response.StatusCode}");
            }
        }
    }
}