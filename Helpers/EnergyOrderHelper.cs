using RestSharp;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ENSEKAutomationTests.Helpers
{
    public static class EnergyOrderHelper
    {
        public static async Task<string?> BuyEnergyAndGetOrderIdAsync(RestClient client, int energyId, int quantity)
        {
            var request = new RestRequest($"/ENSEK/buy/{energyId}/{quantity}", Method.Put);
            RestResponse? response = null;
            try
            {
                response = await client.ExecuteAsync(request);

                Console.WriteLine($"EnergyOrderHelper: StatusCode: {response.StatusCode}");
                Console.WriteLine($"EnergyOrderHelper: Content: {response.Content ?? string.Empty}");

                if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
                {
                    // Parse the message field from JSON
                    var json = JObject.Parse(response.Content);
                    var message = json["message"]?.ToString();
                    if (!string.IsNullOrEmpty(message))
                    {
                        // Find the order id in the message string
                        var tag = "Your order id is ";
                        var idx = message.IndexOf(tag, StringComparison.InvariantCulture);
                        if (idx >= 0)
                        {
                            var afterTag = message.Substring(idx + tag.Length);
                            // The order id may be at the end: chop off any trailing period or spaces.
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

            // Return null if unsuccessful
            return null;
        }
    }
}

