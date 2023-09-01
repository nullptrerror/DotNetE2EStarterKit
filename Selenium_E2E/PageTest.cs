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
        Browser = await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });

        // Define your custom user-agent string
        string customUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";

        // Set the user-agent in the context
        var contextOptions = new BrowserNewContextOptions
        {
            UserAgent = customUserAgent
        };
        Context = await Browser.NewContextAsync(contextOptions);

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