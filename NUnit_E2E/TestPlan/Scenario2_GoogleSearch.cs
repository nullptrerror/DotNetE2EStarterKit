using NUnit.Allure.Attributes;
using NUnit.Framework;


namespace NUnit_E2E.TestPlan
{
    [AllureSuite(nameof(TestPlan))]
    public sealed class Scenario2 : PageTest
    {
        [Test]
        [AllureSubSuite(nameof(Scenario2_GoogleSearch))]
        [AllureTag(nameof(Scenario2_GoogleSearch))]
        public async Task Scenario2_GoogleSearch()
        {
            await Page.GotoAsync("https://www.google.com/");

            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByLabel("Search", new() { Exact = true }).FillAsync("Florida");

            await Page.GetByText("Florida", new() { Exact = true }).ClickAsync();
        }
    }
}