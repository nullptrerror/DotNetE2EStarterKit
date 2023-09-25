using Microsoft.Playwright;
using NUnit.Allure.Attributes;
using NUnit.Framework;

namespace NUnit_E2E.TestPlan
{
    [AllureSuite(nameof(TestPlan))]
    public sealed class Scenario4 : PageTest
    {
        [Test]
        [AllureSubSuite(nameof(Scenario4_TicketMaster))]
        [AllureTag(nameof(Scenario4_TicketMaster))]
        public async Task Scenario4_TicketMaster()
        {
            await Page.GotoAsync("https://www.google.com/");

            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByLabel("Search", new() { Exact = true }).FillAsync("ticketmaster");

            await Page.GetByLabel("Google Search").First.ClickAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "Ticketmaster: Buy Verified Tickets for Concerts, Sports ... Ticketmaster https://www.ticketmaster.com" }).ClickAsync();
        }
    }
}