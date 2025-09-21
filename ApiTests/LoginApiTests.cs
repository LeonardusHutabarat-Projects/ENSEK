using Helpers;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ENSEKAutomationTests.ApiTests
{

    /// <summary>
    /// 
    /// This API test is for verifing the login API endpoint POST /ENSEK/login
    /// by sending valid credentials and checking that a successful response is received.
    /// 
    /// A RestClient is initialized with the ENSEK test base URL.
    /// The test sends a POST request with JSON username and password.
    /// It validates that the API call is successful and returns an HTTP success status.
    /// The response is deserialized into a JSON object.
    /// Assertions confirm that the response contains a "message" field 
    /// with the value "Success" and an "access_token" is present and not empty.
    /// The extracted access token and confirmation message are logged to the test output.
    /// If the API call fails or the response cannot be deserialized, the test fails
    /// with detailed error output.
    /// 
    /// </summary>

    [TestFixture]
    public class LoginApiTests
    {
        RestClient _client;

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
        public void PostLogin_ShouldExtractAccessToken()
        {
            var request = new RestRequest(TestDataHelper.LoginHref, Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(new
            {
                username = "test",
                password = "testing"
            });

            var response = _client.Execute(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"ERROR: Status={response.StatusCode} | {response.ErrorException?.Message ?? response.StatusDescription}");
                Assert.Fail($"API call failed: {response.StatusCode} - {response.ErrorMessage}");
            }

            try
            {
                JsonNode? data = JsonSerializer.Deserialize<JsonNode>(response.Content!);

                Assert.That(data, Is.Not.Null, "No login response data received.");
                Assert.That(data!["message"]?.ToString(), Is.EqualTo("Success"), "Login did not return 'Success' message.");

                string? accessToken = data?["access_token"]?.ToString();
                string? message = data?["message"]?.ToString();
                Assert.That(accessToken, Is.Not.Empty, "Access token was not found in the response.");

                Console.WriteLine($"Extracted access token: {accessToken}");
                Console.WriteLine($"Message: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
                Assert.Fail("Deserialization of API response failed.");
            }
        }
    }
}