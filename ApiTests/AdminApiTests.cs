using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace ENSEKAutomationTests.ApiTests
{
    [TestFixture]
    public class AdminApiTests
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
        public async Task ResetTestData_ReturnsTestDataReset()
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
