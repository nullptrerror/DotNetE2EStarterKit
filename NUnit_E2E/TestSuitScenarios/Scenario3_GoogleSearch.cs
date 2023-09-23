using Microsoft.Playwright;
using NUnit.Allure.Attributes;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace NUnit_E2E.TestSuitScenarios
{
    public partial class Tests : PageTest
    {
        [Test]
        [AllureSubSuite(nameof(Scenario3_GoogleSearch))]
        [AllureTag(nameof(Scenario3_GoogleSearch))]
        public async Task Scenario3_GoogleSearch()
        {
            await Page.GotoAsync("https://www.google.com/");

            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByLabel("Search", new() { Exact = true }).FillAsync("texas");

            await Page.Locator("span").Filter(new() { HasTextRegex = new Regex("^Texas$") }).ClickAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "Texas.gov | The Official Website of the State of Texas Texas.gov https://www.texas.gov" }).ClickAsync();

            //  verify theres no other loading activity
            await Page.WaitForLoadStateAsync(LoadState.Load);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);


            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync("Texas");

            await Page.GetByRole(AriaRole.Link, new() { Name = "Search Icon Texans of Texas" }).ClickAsync();

            await Page.GetByRole(AriaRole.Heading, new() { Name = "Texans of Texas", Exact = true }).ClickAsync();

        }
    }
}