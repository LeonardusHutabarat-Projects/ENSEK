namespace Helpers
{

    /// <summary>
    /// This class provides reusable test data and constants for API and UI automation.
    /// Stores sample credentials, IDs, quantities, URLs, and helper data structures
    /// used across test cases to ensure consistency and maintainability.
    /// 
    /// </summary>

    public static class TestDataHelper
    {
        public static int ValidEnergyId => 1;
        public static int ValidQuantity => 10;
        public static string Username => "test";
        public static string Password => "testing";

        public static string TestEmail => "inconsequential.stuff@googlemail.com";
        public static string TestPassword => "2025Gr@ntham";

        public static readonly Dictionary<string, int> EnergyPurchaseData = new()
        {
            { "Gas", 10},
            { "Electricity", 100 },
            { "Oil", 3 }
        };

        public static readonly Dictionary<string, int> InvalidEnergyPurchaseData = new()
        {
            { "Gas", -10},
            { "Electricity", -100 },
            { "Oil", -50 }
        };

        public const string AccountRegisterHref = "/Account/Register";
        public const string ApiUrl = "https://qacandidatetest.ensek.io";
        public const string BuyEnergyHref = "/ENSEK/buy";
        public const string BuyEnergyUrl = "https://ensekautomationcandidatetest.azurewebsites.net/Energy/Buy";
        public const string EnergyHref = "/ENSEK/energy";
        public const string HomeUrl = "https://ensekautomationcandidatetest.azurewebsites.net/";
        public const string LoginHref = "/ENSEK/login";
        public const string MaintenanceImageSrc = "/Content/Images/maintenance-1151312_960_720.png";
        public const string OrderHref = "/ENSEK/orders";
        public const string ResetHref = "/ENSEK/reset";
        public const string SellEnergyHref = "/Energy/Sell";
        public const string SellEnergyUrl = "https://ensekautomationcandidatetest.azurewebsites.net/Energy/Sell";
        
    }
}