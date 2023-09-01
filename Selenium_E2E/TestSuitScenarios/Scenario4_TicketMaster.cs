using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Selenium_E2E.TestSuitScenarios
{
    [TestFixture]
    public partial class Tests : PageTest
    {
        [Test]
        public async Task Scenario4_TicketMaster()
        {
            await Page.GotoAsync("https://www.google.com/");

            await Page.GetByLabel("Search", new() { Exact = true }).ClickAsync();

            await Page.GetByLabel("Search", new() { Exact = true }).FillAsync("ticketmaster");

            await Page.GetByLabel("Google Search").First.ClickAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "Ticketmaster: Buy Verified Tickets for Concerts, Sports ... Ticketmaster https://www.ticketmaster.com" }).ClickAsync();

            // await Page.GetByPlaceholder("Search for artists, venues, and events").ClickAsync();
            // 
            // await Page.GetByPlaceholder("Search for artists, venues, and events").FillAsync("Tayler Swift");
            // 
            // await Page.GetByRole(AriaRole.Button, new() { Name = "Search", Exact = true }).ClickAsync();
            // 
            // await Page.GetByRole(AriaRole.Link, new() { Name = "Find tickets Taylor Swift | The Eras Tour Miami, FL Hard Rock Stadium 10/18/24, 7:00 PM" }).ClickAsync();

        }
    }
}