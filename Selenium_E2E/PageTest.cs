using PlaywrightClass = Microsoft.Playwright.Playwright;
using Microsoft.Playwright;
using NUnit.Framework;
using Selenium_E2E.Settings;

namespace Selenium_E2E;

public class PageTest
{
    internal IPlaywright Playwright { get; set; } = null!;
    internal IBrowser Browser { get; set; } = null!;
    internal IBrowserContext Context { get; set; } = null!;
    internal IPage Page { get; set; } = null!;
    internal TestAppSettings configuration = null!;


    [SetUp]
    public async Task BaseSetup()
    {
        Playwright = await PlaywrightClass.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        Context = await Browser.NewContextAsync();
        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task BaseTearDown()
    {
        await Context.CloseAsync();
        await Browser.CloseAsync();
    }

    [OneTimeTearDown]
    public void BaseOneTimeTearDown()
    {
        Playwright.Dispose();
    }
}