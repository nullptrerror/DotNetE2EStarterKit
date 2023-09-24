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
    /// Constants for Page object
    /// </summary>
    internal static class PageConsts
    {
        /// <summary>
        /// Constants for reflection of Page.Video object
        /// </summary>
        internal static class VideoReflection
        {
            /// <summary>
            /// The name of the _artifactTcs field in Page.Video._artifactTcs
            /// </summary>
            internal const string _artifactTcs = nameof(_artifactTcs);
            /// <summary>
            /// The name of the Task property Page.Video._artifactTcs.Task
            /// </summary>
            internal const string Task = nameof(Task);
            /// <summary>
            /// The name of the Result property Page.Video._artifactTcs.Task.Result
            /// </summary>
            internal const string Result = nameof(Result);
            /// <summary>
            /// The name of the AbsolutePath property Page.Video._artifactTcs.Task.Result.AbsolutePath
            /// </summary>
            internal const string AbsolutePath = nameof(AbsolutePath);
            /// <summary>
            /// The error message template for when the video absolute path cannot be fetched
            /// </summary>
            internal const string ErrorMessageTemplate = "Error fetching video absolute path: {0}";
        }
        /// <summary>
        /// Constants for Page object
        /// </summary>
        internal static class Browser
        {
            /// <summary>
            /// The directory name where the files from the test will be saved
            /// </summary>
            internal const string AssetsDir = "assets";
            /// <summary>
            /// The video type
            /// </summary>
            internal const string VideoType = "video/webm";
            /// <summary>
            /// The template for the trace file name
            /// </summary>
            internal const string TraceFileTypeTemplate = "{0}.zip";
            /// <summary>
            /// The options to use when waiting for the page to load during teardown
            /// </summary>
            internal static readonly PageWaitForLoadStateOptions TeardownPageWaitForLoadStateOptions = new() { Timeout = 5000 };
            /// <summary>
            /// The user agent to use for the browser
            /// </summary>
            internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
            /// <summary>
            /// The tracing type
            /// </summary>
            internal const string TracingType = "application/zip";
        }
    }
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
    /// The absolute path to the assets directory
    /// </summary>
    internal string AbsoluteAssetsDir
    {
        get => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, PageConsts.Browser.AssetsDir);
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
            UserAgent = PageConsts.Browser.UserAgent,
            RecordVideoDir = AbsoluteAssetsDir,
            RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
        });
        // Start tracing before creating / navigating a page.
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true,
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
            await Page.WaitForLoadStateAsync(LoadState.Load, PageConsts.Browser.TeardownPageWaitForLoadStateOptions);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, PageConsts.Browser.TeardownPageWaitForLoadStateOptions);
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, PageConsts.Browser.TeardownPageWaitForLoadStateOptions);
        }
        catch (Exception ex)
        {
            // Handle or log the exception as appropriate
            Console.WriteLine($"Error waiting for load state: {ex.Message}");
        }

        // Get filename without extension
        var tracingFilename = string.Format(PageConsts.Browser.TraceFileTypeTemplate, Path.Combine(Path.GetFileNameWithoutExtension(videoPath)));

        // Stop tracing and export it into a zip archive.
        await Context.Tracing.StopAsync(new()
        {
            Path = tracingFilename
        });

        // If we close the context before we get the video name it will be null and not tied to allure report test
        await Context.CloseAsync();
        await Browser.CloseAsync();

        // Playwright should give us the name and even absolute path of the video file.
        // I will suggest it or even see if I can implement it myself.
        // We could call it before or after close so it would be a cleaner solution.
        // var videoPath = Page.Video.Path;

        // Add the video to the allure report, must be done after the context is closed, otherwise the video file will be locked.
        AllureLifecycle.Instance.AddAttachment(TestContext.CurrentContext.Test.MethodName ?? string.Empty, PageConsts.Browser.VideoType, videoPath);
        // Add the tracing to the allure report
        AllureLifecycle.Instance.AddAttachment(TestContext.CurrentContext.Test.MethodName ?? string.Empty, PageConsts.Browser.TracingType, tracingFilename);
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
            var _artifactTcsField = Page?.Video?.GetType().GetField(PageConsts.VideoReflection._artifactTcs, BindingFlags.NonPublic | BindingFlags.Instance);
            var _artifactTcsValue = _artifactTcsField?.GetValue(Page?.Video);

            // Get the Task property and its value
            var taskProperty = _artifactTcsValue?.GetType().GetProperty(PageConsts.VideoReflection.Task);
            var taskValue = taskProperty?.GetValue(_artifactTcsValue) as Task;

            // Get the Result property and its value
            var taskResultProperty = taskValue?.GetType().GetProperty(PageConsts.VideoReflection.Result);
            var artifactResult = taskResultProperty?.GetValue(taskValue);

            // Get the AbsolutePath property and its value
            var absolutePathProperty = artifactResult?.GetType().GetProperty(PageConsts.VideoReflection.AbsolutePath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var absolutePathValue = absolutePathProperty?.GetValue(artifactResult) as string;

            return absolutePathValue ?? string.Empty;
        }
        catch (Exception ex)
        {
            // Handle or log the exception as appropriate
            Console.WriteLine(string.Format(PageConsts.VideoReflection.ErrorMessageTemplate, ex.Message));
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