using Microsoft.Playwright;
using NUnit.Framework;
using Serilog;
using System.Collections.Concurrent;

namespace NUnit_E2E.TestFixture.PlaywrightTest;

public class BrowserTest
{
    protected static readonly ConcurrentDictionary<string, ScopedTestContext> ScopedTestContextStorage = new ConcurrentDictionary<string, ScopedTestContext>();
    protected static ILogger Logger { get; private set; } = null!;
    protected static IPlaywright Playwright { get; private set; } = null!;
    protected static IBrowser Browser { get; private set; } = null!;
    protected static IBrowserContext Context => ScopedTestContextStorage[TestContext.CurrentContext.Test.ID].Context;
    protected static IPage Page => ScopedTestContextStorage[TestContext.CurrentContext.Test.ID].Page;
    public BrowserTest()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithThreadId()
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{ThreadId}] {Message:lj         {NewLine}        {Exception}")
            .WriteTo.File("assets/log.txt", rollingInterval: RollingInterval.Hour, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        Log.Information($"BrowserTest {TestContext.CurrentContext.Test.ID}");
    }
    [SetUp]
    public async Task SetUp()
    {
        Log.Information($"SetUp {TestContext.CurrentContext.Test.ID}");
        try
        {
            await Environment.SetUpAsync(ScopedTestContextStorage, Browser, TestContext.CurrentContext.Test.ID);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Exception in SetUp {TestContext.CurrentContext.Test.ID}");
        }
    }
    [TearDown]
    public async Task TearDown()
    {
        Log.Information($"TearDown {TestContext.CurrentContext.Test.ID}");
        try
        {
            await Environment.TearDownAsync(ScopedTestContextStorage, TestContext.CurrentContext.Test.ID);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Exception in TearDown {TestContext.CurrentContext.Test.ID}");
        }
    }
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        (Playwright, Browser) = await Environment.OneTimeSetUpAsync();
        Log.Information(nameof(OneTimeSetUp) + " " + TestContext.CurrentContext.Test.ID);
    }
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Environment.OneTimeTearDownAsync(Playwright, Browser);
        Log.Information(nameof(OneTimeTearDown) + " " + TestContext.CurrentContext.Test.ID);
        Log.CloseAndFlush();
    }
}