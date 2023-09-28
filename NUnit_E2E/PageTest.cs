using Microsoft.Playwright;
using NUnit.Framework;
using NUnit_E2E.Settings;
using System.Reflection;
using Allure.Net.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit_E2E.Helpers;

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
    internal sealed class PageConsts
    {
        /// <summary>
        /// Constants for Page object
        /// </summary>
        internal sealed class Browser
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
            internal const string TraceFileExtension = ".playwright.trace.zip";
            /// <summary>
            /// The template for the trace file name
            /// </summary>
            internal const string VideoFileExtension = ".playwright.video.webm";
            /// <summary>
            /// The error message template for when the page cannot be closed
            /// </summary>
            internal const string WaitForLoadStateErrorMessageTemplate = "Error waiting for load state: {0}";
            /// <summary>
            /// The page wait for load state options
            /// <br/>
            /// <inheritdoc cref="PageWaitForLoadStateOptions.Timeout"/>
            /// </summary>
            /// <inheritdoc cref="PageWaitForLoadStateOptions.Timeout"/>
            internal static readonly PageWaitForLoadStateOptions TeardownPageWaitForLoadStateOptions = new PageWaitForLoadStateOptions()
            {
                Timeout = 5000
            };
            /// <summary>
            /// The user agent to use for the browser
            /// </summary>
            internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
            /// <summary>
            /// The tracing type
            /// </summary>
            internal const string TracingType = "application/zip";
            /// <summary>
            /// <inheritdoc cref="BrowserTypeLaunchOptions.Args"/>
            /// </summary>
            /// <inheritdoc cref="BrowserTypeLaunchOptions.Args"/>
            internal static readonly IEnumerable<string>? BrowserTypeLaunchOptionsArgs = new[] {
                // "--start-maximized",           // Maximize the browser window
                WindowHelper.WindowSizeCommand,
                // "--window-size=1920,1080",           // Maximize the browser window
                // "--start-in-incognito",        // Start browser in incognito mode
                // "--force-dark-mode",           // Force dark mode
                // "--enable-features=WebUIDarkMode"  // Enable dark mode feature
            };
            /// <summary>
            /// The browser type launch options
            /// </summary>
            internal static readonly BrowserTypeLaunchOptions BrowserTypeLaunchOptions = new BrowserTypeLaunchOptions()
            {
                Headless = Headless,
                Devtools = DevTools,
                Args = BrowserTypeLaunchOptionsArgs,
                DownloadsPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, AssetsDir),
                TracesDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, AssetsDir),
                // SlowMo = 100,
                // Timeout = 10000,
            };
            /// <summary>
            /// The record video size
            /// </summary>
            internal static RecordVideoSize RecordVideoSize
            {
                get => new RecordVideoSize()
                {
                    Width = WindowHelper.CurrentHorizontalResolution,
                    Height = WindowHelper.CurrentVerticalResolution
                };
            }
            /// <summary>
            /// The browser new context options
            /// </summary>
            internal static readonly BrowserNewContextOptions BrowserNewContextOptions = new BrowserNewContextOptions
            {
                UserAgent = UserAgent,
                RecordVideoDir = AbsoluteAssetsDir,
                RecordVideoSize = RecordVideoSize
            };
            /// <summary>
            /// The tracing start options
            /// </summary>
            internal static readonly TracingStartOptions TracingStartOptions = new TracingStartOptions()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            };
        }
    }
    /// <summary>
    /// <inheritdoc cref="IPlaywright"/>
    /// </summary>
    internal IPlaywright Playwright { get; set; } = null!;
    /// <summary>
    /// <inheritdoc cref="IBrowser"/>
    /// </summary>
    internal IBrowser Browser { get; set; } = null!;
    /// <summary>
    /// <inheritdoc cref="IBrowserContext"/>
    /// </summary>
    internal IBrowserContext Context { get; set; } = null!;
    /// <summary>
    /// <inheritdoc cref="IPage"/>
    /// </summary>
    internal IPage Page { get; set; } = null!;
    /// <summary>
    /// The test app settings object
    /// </summary>
    internal TestAppSettings configuration = null!;
    /// <summary>
    /// <inheritdoc cref="BrowserTypeLaunchOptions.Headless"/>
    /// </summary>
    internal const bool Headless = false; // !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(nameof(Headless).ToUpper()));
    /// <summary>
    /// <inheritdoc cref="BrowserTypeLaunchOptions.Devtools"/>
    /// </summary>
    internal const bool DevTools = false; // !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(nameof(DevTools).ToUpper()));
    /// <summary>
    /// <inheritdoc cref="Assembly.Location"/>
    /// <br/>
    /// <inheritdoc cref="PageConsts.Browser.AssetsDir"/>
    /// </summary>
    /// <inheritdoc cref="PageConsts.Browser.AssetsDir"/>
    /// <inheritdoc cref="Assembly.Location"/>
    /// <inheritdoc cref="Path.Combine"/>
    /// <inheritdoc cref="Path.GetDirectoryName"/>
    internal static string AbsoluteAssetsDir
    {
        get => Path.Combine(TestContext.CurrentContext.WorkDirectory, PageConsts.Browser.AssetsDir);
    }
    /// <summary>
    /// Setup the test
    /// </summary>
    /// <returns></returns>
    [SetUp]
    public async Task BaseSetup()
    {
        WindowHelper.GetScreenDimensionsAndPosition();
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(PageConsts.Browser.BrowserTypeLaunchOptions);
        Context = await Browser.NewContextAsync(PageConsts.Browser.BrowserNewContextOptions);
        await Context.Tracing.StartAsync(PageConsts.Browser.TracingStartOptions);
        Page = await Context.NewPageAsync();
        await Page.SetViewportSizeAsync(WindowHelper.CurrentHorizontalResolution, WindowHelper.CurrentVerticalResolution);
    }
    /// <summary>
    /// Tear down the test
    /// </summary>
    /// <returns></returns>
    [TearDown]
    public async Task BaseTearDown()
    {
        // Wait for the page to finish loading
        try
        {
            if (!Page.IsClosed) await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, PageConsts.Browser.TeardownPageWaitForLoadStateOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format(PageConsts.Browser.WaitForLoadStateErrorMessageTemplate, ex.Message));
        }
        // Generate a common file name for the trace and video files
        var commonFileName = Guid.NewGuid().ToString("N");
        // Get the absolute path for the trace and video files
        var absoluteTraceFilePath = string.Concat(AbsoluteAssetsDir,
                                                    Path.DirectorySeparatorChar,
                                                    commonFileName,
                                                    PageConsts.Browser.TraceFileExtension);
        var absoluteVideoFilePath = string.Concat(AbsoluteAssetsDir,
                                                    Path.DirectorySeparatorChar,
                                                    commonFileName,
                                                    PageConsts.Browser.VideoFileExtension);
        // Stop tracing and export it into a zip archive.
        await Context.Tracing.StopAsync(new TracingStopOptions()
        {
            Path = absoluteTraceFilePath
        });

        // Close and clean up the browser and context
        await Context.CloseAsync();
        await Browser.CloseAsync();

        // Add the tracing to the allure report
        AllureLifecycle.Instance.AddAttachment(string.Concat(
                PageConsts.Browser.TracingType, " - ", TestContext.CurrentContext.Test.MethodName),
                PageConsts.Browser.TracingType,
            absoluteTraceFilePath);

        if (Page?.Video != null)
        {
            // Rename the video file to match the test name
            File.Move(await Page.Video.PathAsync(), absoluteVideoFilePath);

            AllureLifecycle.Instance.AddAttachment(string.Concat(
                PageConsts.Browser.VideoType, " - ", TestContext.CurrentContext.Test.MethodName),
                PageConsts.Browser.VideoType,
            absoluteVideoFilePath);
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