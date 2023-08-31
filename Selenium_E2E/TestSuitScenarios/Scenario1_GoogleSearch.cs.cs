using NUnit.Framework;

namespace Selenium_E2E.TestSuitScenarios
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public partial class Tests : PageTest
    {
        [Test]
        public async Task Scenario1_GoogleSearch()
        {
            await Page.GotoAsync("https://www.google.com/");

            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByLabel("Search", new() { Exact = true }).FillAsync("Texas");

            await Page.GetByText("Texas", new() { Exact = true }).ClickAsync();

            // Wait ten seconds
            await Page.WaitForTimeoutAsync(10000);
        }
    }
}