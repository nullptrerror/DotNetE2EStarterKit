using NUnit.Framework;

namespace Selenium_E2E.TestSuitScenarios
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task Scenario2_GoogleSearch()
        {
            await Page.GotoAsync("https://www.google.com/");

            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByLabel("Search", new() { Exact = true }).FillAsync("Florida");

            await Page.GetByText("Florida", new() { Exact = true }).ClickAsync();

            // Wait ten seconds
            await Page.WaitForTimeoutAsync(10000);
        }
    }
}