using Helpers;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

namespace ENSEKAutomationTests.Helpers
{

    /// <summary>
    /// 
    /// This class sends a PUT request to purchase a specified quantity of energy by its ID,
    /// then attempts to parse and return the generated order ID from the response.
    /// Logs the status code, response content, and any exceptions encountered.
    /// Returns null if the request fails, if no content is returned,
    /// or if an order ID cannot be extracted from the response.
    /// 
    /// </summary>
    /// 
    /// <param name="client">The RestSharp <see cref="RestClient"/> used to send the request.</param>
    /// <param name="energyId">The numeric identifier of the energy type to purchase.</param>
    /// <param name="quantity">The amount of energy units to buy.</param>
    /// <returns>The extracted order ID as a string if successful; otherwise null.</returns>

    public static class EnergyOrderHelper
    {
        public static async Task<string?> BuyEnergyAndGetOrderIdAsync(RestClient client, int energyId, int quantity)
        {
            var request = new RestRequest($"{TestDataHelper.BuyEnergyHref}/{energyId}/{quantity}", Method.Put);
            RestResponse? response = null;
            try
            {
                response = await client.ExecuteAsync(request);

                Console.WriteLine($"EnergyOrderHelper: StatusCode: {response.StatusCode}");
                Console.WriteLine($"EnergyOrderHelper: Content: {response.Content ?? string.Empty}");

                if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
                {
                    var json = JObject.Parse(response.Content);
                    var message = json["message"]?.ToString();
                    if (!string.IsNullOrEmpty(message))
                    {

                        var tag = "Your order id is ";
                        var idx = message.IndexOf(tag, StringComparison.InvariantCulture);
                        if (idx >= 0)
                        {
                            var afterTag = message.Substring(idx + tag.Length);
                            var orderId = afterTag.TrimEnd('.', ' ', '\n', '\r');
                            return orderId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EnergyOrderHelper: Exception: {ex.Message}");
            }

            return null;
        }
    }
}