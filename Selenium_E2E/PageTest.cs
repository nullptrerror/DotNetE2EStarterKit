using MicrosoftPlaywrightPlaywright = Microsoft.Playwright.Playwright;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit_E2E.Settings;

namespace NUnit_E2E;

public class PageTest
{
    internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
    internal const string Headless = "HEADLESS";
    internal IPlaywright Playwright { get; set; } = null!;
    internal IBrowser Browser { get; set; } = null!;
    internal IBrowserContext Context { get; set; } = null!;
    internal IPage Page { get; set; } = null!;
    internal TestAppSettings configuration = null!;

    [SetUp]
    public async Task BaseSetup()
    {
        Playwright = await MicrosoftPlaywrightPlaywright.CreateAsync();
        Browser = await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(Headless)) });
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions { UserAgent = UserAgent });
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