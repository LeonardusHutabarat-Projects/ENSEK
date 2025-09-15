namespace Helpers
{
    public static class TestDataHelper
    {
        public static int ValidEnergyId => 1;
        public static int ValidQuantity => 10;
        public static string TestEmail => "inconsequential.stuff@googlemail.com";
        public static string TestPassword => "2025Gr@ntham";

        public static readonly Dictionary<string, int> EnergyPurchaseData = new()
        {
            { "Gas", 10 },
            { "Electricity", 5 },
            { "Oil", 3 }
        };

        public const string HomeUrl = "https://ensekautomationcandidatetest.azurewebsites.net/";
        public const string BuyEnergyUrl = "https://ensekautomationcandidatetest.azurewebsites.net/Energy/Buy";
        public const string AccountRegisterHref = "/Account/Register";
        public const string SellEnergyHref = "/Energy/Sell";
        public const string SellEnergyUrl = "https://ensekautomationcandidatetest.azurewebsites.net/Energy/Sell";
        public const string MaintenanceImageSrc = "/Content/Images/maintenance-1151312_960_720.png";
    }
}

