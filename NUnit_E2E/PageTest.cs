using Microsoft.Playwright;
using NUnit.Framework;
using NUnit_E2E.Settings;
using System.Reflection;
using Allure.Net.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;

namespace NUnit_E2E;

/// <summary>
/// The base test class for all tests
/// </summary>
[AllureNUnit]
[AllureParentSuite(AllureSuite)]
public class PageTest
{
    /// <summary>
    /// The name of the root suite
    /// </summary>
    internal const string AllureSuite = "Root Suite";
    /// <summary>
    /// The directory name where the video files will be stored
    /// </summary>
    internal const string VideoDir = "videos";
    /// <summary>
    /// The user agent to use for the browser
    /// </summary>
    internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
    /// <summary>
    /// The Playwright object
    /// </summary>
    internal IPlaywright Playwright { get; set; } = null!;
    /// <summary>
    /// The browser object
    /// </summary>
    internal IBrowser Browser { get; set; } = null!;
    /// <summary>
    /// The browser context object
    /// </summary>
    internal IBrowserContext Context { get; set; } = null!;
    /// <summary>
    /// The page object
    /// </summary>
    internal IPage Page { get; set; } = null!;
    /// <summary>
    /// The test app settings object
    /// </summary>
    internal TestAppSettings configuration = null!;
    /// <summary>
    /// Whether or not to run the browser in headless mode
    /// </summary>
    internal bool Headless => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(nameof(Headless).ToUpper()));
    /// <summary>
    /// The absolute path to the video directory
    /// </summary>
    internal string RecordVideoDir
    {
        get => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, VideoDir);
    }

    /// <summary>
    /// Setup the test
    /// </summary>
    /// <returns></returns>
    [SetUp]
    public async Task BaseSetup()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = Headless });
        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            UserAgent = UserAgent,
            RecordVideoDir = RecordVideoDir,
            RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
        });
        Page = await Context.NewPageAsync();
    }

    /// <summary>
    /// Tear down the test
    /// </summary>
    /// <returns></returns>
    [TearDown]
    public async Task BaseTearDown()
    {
        // Get video file path with name from the page object before we close the context, otherwise the video file name won't be in the Page.Video object
        var videoPath = _getVideoAbsolutePath();

        try
        {
            //  verify theres no other loading activity
            await Page.WaitForLoadStateAsync(LoadState.Load);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
        catch (Exception ex)
        {
            // Handle or log the exception as appropriate
            Console.WriteLine($"Error waiting for load state: {ex.Message}");
        }

        // If we close the context before we get the video name it will be null and not tied to allure report test
        await Context.CloseAsync();
        await Browser.CloseAsync();

        // Allure should give us the name and even absolute path of the video file. I will suggest it to their team and see if I can implement it myself. We could call it before or after close so it would be a cleaner solution.
        // var videoPath = Page.Video.Path;

        // Add the video to the allure report, must be done after the context is closed, otherwise the video file will be locked.
        AllureLifecycle.Instance.AddAttachment($"{TestContext.CurrentContext.Test.MethodName} Video", "video/webm", videoPath);
    }

    /// <summary>
    /// Get the absolute path to the video file
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private string _getVideoAbsolutePath()
    {
        // Ensure we have a page object
        if (Page == null) throw new ArgumentNullException(nameof(Page));

        try
        {
            // Get the non-public _artifactTcs field from the Video object
            var _artifactTcsField = Page?.Video?.GetType().GetField("_artifactTcs", BindingFlags.NonPublic | BindingFlags.Instance);
            var _artifactTcsValue = _artifactTcsField?.GetValue(Page?.Video);

            // Get the Task property and its value
            var taskProperty = _artifactTcsValue?.GetType().GetProperty("Task");
            var taskValue = taskProperty?.GetValue(_artifactTcsValue) as Task;

            // Get the Result property and its value
            var taskResultProperty = taskValue?.GetType().GetProperty("Result");
            var artifactResult = taskResultProperty?.GetValue(taskValue);

            // Get the AbsolutePath property and its value
            var absolutePathProperty = artifactResult?.GetType().GetProperty("AbsolutePath", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var absolutePathValue = absolutePathProperty?.GetValue(artifactResult) as string;

            return absolutePathValue ?? string.Empty;
        }
        catch (Exception ex)
        {
            // Handle or log the exception as appropriate
            Console.WriteLine($"Error fetching video absolute path: {ex.Message}");
            return string.Empty;  // or rethrow, based on your requirements
        }
    }

    /// <summary>
    /// Tear down the test
    /// </summary>
    [OneTimeTearDown]
    public void BaseOneTimeTearDown()
    {
        Playwright.Dispose();
    }
}