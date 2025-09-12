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
    }
}

