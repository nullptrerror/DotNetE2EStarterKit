using NUnit.Allure.Attributes;

namespace NUnit_E2E.TestPlan
{
    [AllureSuite(nameof(Tests))]
    public partial class Tests : PageTest
    {
        public static string BaseUrl { get; set; } = "https://www.google.com/";
    }
}
