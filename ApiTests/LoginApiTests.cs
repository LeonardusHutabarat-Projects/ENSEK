using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using NUnit.Framework;
using RestSharp;

namespace ENSEKAutomationTests.ApiTests
{
    [TestFixture]
    public class LoginApiTests
    {
        RestClient _client;

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
        public void PostLogin_ShouldExtractAccessToken()
        {
            var request = new RestRequest("/ENSEK/login", Method.Post);
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
